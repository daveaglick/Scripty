// The following environment variables need to be set for Publish target:
// NUGET_API_KEY
// SCRIPTY_GITHUB_TOKEN

// Publishing workflow:
// - Update ReleaseNotes.md
// - Update the version in Scripty.CustomTool/source.extension.vsixmanifest
// - Run a normal build with Cake to set SolutionInfo.cs in the repo ("build.cmd")
// - Run a Publish build with Cake ("build.cmd --target Publish")
// - No need to add a version tag to the repo - added by GitHub on publish
// - Manually upload the .vsix in src\artifacts to the Visual Studio Gallery

#addin "Cake.FileHelpers"
#addin "Octokit"
using Octokit;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var isLocal = BuildSystem.IsLocalBuild;
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;
var buildNumber = AppVeyor.Environment.Build.Number;

var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");

var version = releaseNotes.Version.ToString();
var semVersion = version + (isLocal ? string.Empty : string.Concat("-build-", buildNumber));

var buildDir = Directory("./src/Scripty/bin") + Directory(configuration);
var buildResultDir = Directory("./build") + Directory(semVersion);
var nugetRoot = buildResultDir + Directory("nuget");
var binDir = buildResultDir + Directory("bin");

var zipFile = "Scripty-v" + semVersion + ".zip";

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    Information("Building version {0} of Scripty.", semVersion);
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(new DirectoryPath[] { buildDir, buildResultDir, binDir, nugetRoot });
    });

Task("Restore-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore("./src/Scripty.sln");
    });

Task("Patch-Assembly-Info")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
    {
        var file = "./src/SolutionInfo.cs";
        CreateAssemblyInfo(file, new AssemblyInfoSettings {
            Product = "Scripty",
            Copyright = "Copyright \xa9 Scripty Contributors",
            Version = version,
            FileVersion = version,
            InformationalVersion = semVersion
        });
    });

Task("Build")
    .IsDependentOn("Patch-Assembly-Info")
    .Does(() =>
    {
        MSBuild("./src/Scripty.sln", new MSBuildSettings()
            .SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Minimal)
            .SetMSBuildPlatform(MSBuildPlatform.x86)
        );
    });

Task("Copy-Files")
    .IsDependentOn("Build")
    .Does(() =>
    {
        CopyDirectory(buildDir, binDir);
        CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binDir);
    });

Task("Zip-Files")
    .IsDependentOn("Copy-Files")
    .Does(() =>
    {
        var zipPath = buildResultDir + File(zipFile);
        var files = GetFiles(binDir.Path.FullPath + "/**/*");
        Zip(binDir, zipPath, files);
    });

Task("Create-Library-Packages")
    .IsDependentOn("Build")
    .Does(() =>
    {        
        // Get the set of nuspecs to package
        List<FilePath> nuspecs = new List<FilePath>(GetFiles("./src/Scripty.*/*.nuspec"));
        
        // Package all nuspecs
        foreach (var nuspec in nuspecs)
        {
            // Common settings
            var nuGetPackSettings = new NuGetPackSettings
            {
                Version = semVersion,
                BasePath = nuspec.GetDirectory(),
                OutputDirectory = nugetRoot,
                Symbols = false,
                NoPackageAnalysis = true,
                Properties = new Dictionary<string, string>
                {
                    { "Configuration", configuration }
                }
            };
            
            // Add the tools property to the MSBuild package
            if(nuspec.GetFilenameWithoutExtension().FullPath == "Scripty.MsBuild")
            {
                nuGetPackSettings.ArgumentCustomization = args => args.Append("-Tool");
            }
                
            // Do the packing
            NuGetPack(nuspec.ChangeExtension(".csproj"), nuGetPackSettings);
        }
    });

Task("Create-Tools-Package")
    .IsDependentOn("Build")
    .Does(() =>
    {        
        var nuspec = GetFiles("./src/Scripty/*.nuspec").FirstOrDefault();
        if (nuspec == null)
        {            
            throw new InvalidOperationException("Could not find tools nuspec.");
        }
        var pattern = string.Format("bin\\{0}\\**\\*", configuration);  // This is needed to get around a Mono scripting issue (see #246, #248, #249)
        NuGetPack(nuspec, new NuGetPackSettings
        {
            Version = semVersion,
            BasePath = nuspec.GetDirectory(),
            OutputDirectory = nugetRoot,
            Symbols = false,
            Files = new [] 
            { 
                new NuSpecContent 
                { 
                    Source = pattern,
                    Target = "tools"
                } 
            }
        });
    });
    
Task("Test-MsBuild")
    .IsDependentOn("Create-Packages")
    .Does(() =>
    {
        if(DirectoryExists("./src/Scripty.MsBuild.Test/packages"))
        {
            DeleteDirectory("./src/Scripty.MsBuild.Test/packages", true);
        }
        NuGetRestore("./src/Scripty.MsBuild.Test/Scripty.MsBuild.Test.sln");
        NuGetInstall("Scripty.MsBuild", new NuGetInstallSettings
        {
            NoCache = true,
            Source = new [] { "file:///" + MakeAbsolute(nugetRoot).FullPath },
            ExcludeVersion = true,
            Prerelease = true,
            OutputDirectory = "./src/Scripty.MsBuild.Test/packages"
        });      
        MSBuild("./src/Scripty.MsBuild.Test/Scripty.MsBuild.Test.sln", new MSBuildSettings()
            .SetConfiguration("Debug")
            .SetVerbosity(Verbosity.Minimal)
            //.SetVerbosity(Verbosity.Diagnostic)
        );  
    });
        
Task("Publish-Packages")
    .IsDependentOn("Create-Packages")
    .WithCriteria(() => isLocal)
    // TODO: Add criteria that makes sure this is the master branch
    .Does(() =>
    {
        var apiKey = EnvironmentVariable("NUGET_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Could not resolve NuGet API key.");
        }

        foreach (var nupkg in GetFiles(nugetRoot.Path.FullPath + "/*.nupkg"))
        {
            NuGetPush(nupkg, new NuGetPushSettings 
            {
                ApiKey = apiKey
            });
        }
    });
    
Task("Publish-Release")
    .IsDependentOn("Zip-Files")
    .WithCriteria(() => isLocal)
    // TODO: Add criteria that makes sure this is the master branch
    .Does(() =>
    {
        var githubToken = EnvironmentVariable("SCRIPTY_GITHUB_TOKEN");
        if (string.IsNullOrEmpty(githubToken))
        {
            throw new InvalidOperationException("Could not resolve GitHub token.");
        }
        
        var github = new GitHubClient(new ProductHeaderValue("ScriptyCakeBuild"))
        {
            Credentials = new Credentials(githubToken)
        };
        var release = github.Release.Create("daveaglick", "Scripty", new NewRelease("v" + semVersion) 
        {
            Name = semVersion,
            Body = string.Join(Environment.NewLine, releaseNotes.Notes),
            Prerelease = true,
            TargetCommitish = "master"
        }).Result;
        
        var zipPath = buildResultDir + File(zipFile);
        using (var zipStream = System.IO.File.OpenRead(zipPath.Path.FullPath))
        {
            var releaseAsset = github.Release.UploadAsset(release, new ReleaseAssetUpload(zipFile, "application/zip", zipStream, null)).Result;
        }
    });
    
Task("Update-AppVeyor-Build-Number")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
    {
        AppVeyor.UpdateBuildVersion(semVersion);
    });

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Zip-Files")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
    {
        var artifact = buildResultDir + File(zipFile);
        AppVeyor.UploadArtifact(artifact);
    });
    
//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////
    
Task("Create-Packages")
    .IsDependentOn("Create-Library-Packages")   
    .IsDependentOn("Create-Tools-Package");
    
Task("Package")
    .IsDependentOn("Zip-Files")
    .IsDependentOn("Create-Packages")
    .IsDependentOn("Test-MsBuild");
    
Task("Test")
    .IsDependentOn("Test-MsBuild");

Task("Default")
    .IsDependentOn("Package");

Task("Publish")
    .IsDependentOn("Publish-Packages")
    .IsDependentOn("Publish-Release");
    
Task("AppVeyor")
    .IsDependentOn("Update-AppVeyor-Build-Number")
    .IsDependentOn("Upload-AppVeyor-Artifacts");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
