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
    {
      var settings = new DockerComposeRunSettings {
      };
      var service = "sample";
      var command = "init";

      DockerComposeRun(settings, service, command);
    }

    {
      var settings = new DockerComposeRunSettings {
      };
      var service = "sample";
      var command = "plan";

      DockerComposeRun(settings, service, command);
    }
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
