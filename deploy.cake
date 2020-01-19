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
    var settings = new DockerComposeRunSettings {
    };
    var service = "sample";
    var command = "init";
    DockerComposeRun(settings, service, command);
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    var settings = new DockerComposeRunSettings {
    };
    var service = "sample";
    var command = "plan";
    DockerComposeRun(settings, service, command);
  });

Task("Package")
  .IsDependentOn("Test")
  .Does(() => {
    if (string.IsNullOrEmpty(packageRegistry)) {
      return;
    }

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
    if (string.IsNullOrEmpty(packageRegistry)) {
      return;
    }

    var settings = new DockerImagePushSettings {
    };
    var imageReference = GetSampleImageReference();
    DockerPush(settings, imageReference);
  });

RunTarget(target);
