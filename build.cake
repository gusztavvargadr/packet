#load ./build/cake/core.cake

Task("Restore")
  .IsDependentOn("RestoreCore")
  .Does(() => {
    var settings = new DockerComposeBuildSettings {
    };
    var services = new [] { "terraform" };
    DockerComposeBuild(settings, services);
  });

Task("Build")
  .IsDependentOn("Restore")
  .Does(() => {
    var settings = new DockerComposeBuildSettings {
    };
    var services = new [] { "sample" };
    DockerComposeBuild(settings, services);
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    var settings = new DockerComposeRunSettings {
    };
    var service = "sample";

    var initCommand = "init -backend=false";
    DockerComposeRun(settings, service, initCommand);

    var validateCommand = "validate";
    DockerComposeRun(settings, service, validateCommand);
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
    var imageReference = GetSampleImageReference();
    DockerPush(settings, imageReference);
  });

RunTarget(target);
