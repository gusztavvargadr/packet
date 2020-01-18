#load ./build/cake/core.cake

Task("Restore")
  .IsDependentOn("RestoreCore")
  .Does(() => {
    var settings = new DockerImagePullSettings {
    };
    var imageReference = GetSampleImageReference();
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
    var imageReference = GetSampleImageReference();

    Environment.SetEnvironmentVariable("SAMPLE_REGISTRY", packageRegistry);
    Environment.SetEnvironmentVariable("SAMPLE_NAME", sampleName);
    Environment.SetEnvironmentVariable("SAMPLE_TAG", packageVersion);
    var registryReference = GetSampleImageReference();

    Information($"Tagging '{imageReference}' as '{registryReference}'.");
    DockerTag(imageReference, registryReference);
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
