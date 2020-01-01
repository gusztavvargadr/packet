#load ./build/cake/core.cake

Task("Restore")
  .IsDependentOn("Version")
  .Does(() => {
    var settings = new DockerImagePullSettings {
    };
    var imageReference = GetDockerImageReference();
    DockerPull(settings, imageReference);
  });

Task("Build")
  .IsDependentOn("Restore")
  .Does(() => {
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    var settings = new DockerComposeRunSettings {
    };
    var service = "sample";

    var initCommand = "init";
    DockerComposeRun(settings, service, initCommand);

    var planCommand = "plan";
    DockerComposeRun(settings, service, planCommand);
  });

Task("Package")
  .IsDependentOn("Test")
  .Does(() => {
  });

Task("Publish")
  .IsDependentOn("Package")
  .Does(() => {
    var settings = new DockerImagePushSettings {
    };
    var imageReference = GetDockerImageReference();
    DockerPush(settings, imageReference);
  });

RunTarget(target);
