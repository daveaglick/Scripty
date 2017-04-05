#quick and dirty to remove and re-add the Scripty vsix
$cur = $PSScriptRoot

cd $env:VS140COMNTOOLS\..\IDE

.\vsixInstaller.exe /q /a /u:Scripty.CustomTool

Read-Host -Prompt "Press Enter to once uninstall is complete"
.\VSIXInstaller.exe /a $cur\bin\Debug\Scripty.vsix

cd $cur



 