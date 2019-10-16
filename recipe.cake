#load nuget:?package=Cake.Recipe&version=1.1.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "VCSVersion",
    repositoryOwner: "hgtools",
    repositoryName: "VCSVersion",
    appVeyorAccountName: "hgtools",
    shouldPostToGitter: false,
    shouldPostToSlack: false,
    shouldPostToTwitter: false,
    shouldPostToMicrosoftTeams: false,
    shouldRunCodecov: false,
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