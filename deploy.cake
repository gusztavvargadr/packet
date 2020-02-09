#load ./build/cake/core.cake

Task("Restore")
  .IsDependentOn("RestoreCore")
  .Does(() => {
    {
      var settings = new DockerImagePullSettings {
      };
      var imageReference = GetSampleImageReference();
      DockerPull(settings, imageReference);
    }

    if (packageRegistry == defaultDockerRegistry) {
      var settings = new DockerComposeUpSettings {
        DetachedMode = true
      };
      var services = new [] { "registry" };
      DockerComposeUp(settings, services);
    }
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
    var imageReference = GetSampleImageReference();

    var sampleImage = $"{packageRegistry}sample-{sampleName}:{packageVersion}";
    Environment.SetEnvironmentVariable("SAMPLE_IMAGE", sampleImage);
    Information($"SAMPLE_IMAGE: '{sampleImage}'.");

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
