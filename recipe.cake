#load nuget:?package=Cake.Recipe&version=1.0.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "VCSVersion",
    repositoryOwner: "hgtools",
    repositoryName: "VCSVersion",
    appVeyorAccountName: "hgtools",
    shouldRunCodecov: false,
    shouldRunDotNetCorePack: true,
    shouldRunGitVersion: true,
    solutionFilePath: "./src/VCSVersion.sln");

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(
    context: Context,
    dupFinderExcludePattern: new string[] 
    {
        BuildParameters.RootDirectoryPath + "/src/VCSVersionTests/**/*.cs",
        BuildParameters.RootDirectoryPath + "/src/VCSVersion/**/*.AssemblyInfo.cs"
    });

Build.RunDotNetCore();