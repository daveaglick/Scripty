// The following environment variables need to be set for Publish target:
// NUGET_API_KEY
// SCRIPTY_GITHUB_TOKEN

// Publishing workflow:
// - Update ReleaseNotes.md
// - Run a normal build with Cake to set SolutionInfo.cs in the repo ("build.cmd")
// - Run a Publish build with Cake ("build.cmd --target Publish")
// - No need to add a version tag to the repo - added by GitHub on publish

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

var msBuildBuildDir = Directory("./src/Scripty.MsBuild/bin") + Directory(configuration);
var buildResultDir = Directory("./build") + Directory(semVersion);

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
        CleanDirectories(new DirectoryPath[] { msBuildBuildDir, buildResultDir });
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
        );
    });

Task("Create-Packages")
    .IsDependentOn("Build")
    .Does(() =>
    {        
        // Get the set of nuspecs to package
        List<FilePath> nuspecs = new List<FilePath>(GetFiles("./src/**/*.nuspec"));
        
        // Package all nuspecs
        foreach (var nuspec in nuspecs)
        {
            NuGetPack(nuspec.ChangeExtension(".csproj"), new NuGetPackSettings
            {
                Version = semVersion,
                BasePath = nuspec.GetDirectory(),
                OutputDirectory = buildResultDir,
                Symbols = false,
                Properties = new Dictionary<string, string>
                {
                    { "Configuration", configuration }
                },
                ArgumentCustomization = args => args.Append("-Tool")
            });
        }
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

        foreach (var nupkg in GetFiles(buildResultDir.Path.FullPath + "/*.nupkg"))
        {
            NuGetPush(nupkg, new NuGetPushSettings 
            {
                ApiKey = apiKey
            });
        }
    });
    
Task("Publish-Release")
    .WithCriteria(() => isLocal)
    // TODO: Add criteria that makes sure this is the master branch
    .Does(() =>
    {
        var githubToken = EnvironmentVariable("SCRIPTBUILDTOOLS_GITHUB_TOKEN");
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
    });
    
Task("Update-AppVeyor-Build-Number")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
    {
        AppVeyor.UpdateBuildVersion(semVersion);
    });
    
//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////
    
Task("Package")
    .IsDependentOn("Create-Packages");

Task("Default")
    .IsDependentOn("Package");    

Task("Publish")
    .IsDependentOn("Publish-Packages")
    .IsDependentOn("Publish-Release");
    
Task("AppVeyor")
    .IsDependentOn("Update-AppVeyor-Build-Number");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
