#load ./build/cake/core.cake

Task("Restore")
  .IsDependentOn("Version")
  .Does(() => {
    var settings = new DockerComposeRunSettings {
    };
    var service = "sample";
    var command = "init";

    DockerComposeRun(settings, service, command);
  });

Task("Build")
  .IsDependentOn("Restore")
  .Does(() => {
    var settings = new DockerComposeRunSettings {
    };
    var service = "sample";
    var command = "plan";

    DockerComposeRun(settings, service, command);
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    var settings = new DockerComposeRunSettings {
    };
    var service = "sample";
    var command = "apply";

    DockerComposeRun(settings, service, command);
  });

Task("Package")
  .IsDependentOn("Test")
  .Does(() => {
  });

Task("Publish")
  .IsDependentOn("Package")
  .Does(() => {
  });

RunTarget(target);
