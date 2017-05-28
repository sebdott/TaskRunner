#tool "nuget:?package=xunit.runner.console"

var target = Argument("target", "Build");
var solutionPath = Argument("solutionPath", "");

Task("Default")
	.IsDependentOn("Build");

Task("Build")
  .Does(() =>
{

var solutions = GetFiles(solutionPath);
// Restore all NuGet packages.
foreach(var solution in solutions)
{
    Information("Restoring {0}", solution);
    NuGetRestore(solution);
}
  MSBuild(solutionPath);
});


RunTarget(target);