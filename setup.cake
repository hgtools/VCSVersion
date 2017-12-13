#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "VCSVersion",
    repositoryOwner: "vCipher",
    repositoryName: "VCSVersion",
    appVeyorAccountName: "vCipher",
    shouldRunCodecov: false,
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