#load ./build/cake/core.cake

Task("Restore")
  .IsDependentOn("RestoreCore")
  .Does(() => {
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
    {
      var settings = new DockerComposeRunSettings {
      };
      var service = "sample";
      var command = "init -backend=false";

      DockerComposeRun(settings, service, command);
    }

    {
      var settings = new DockerComposeRunSettings {
      };
      var service = "sample";
      var command = "validate";

      DockerComposeRun(settings, service, command);
    }
  });

Task("Package")
  .IsDependentOn("Test")
  .Does(() => {
    var sampleImageReference = GetSampleImageReference();
    var artifactImageReference = GetArtifactImageReference();

    DockerTag(sampleImageReference, artifactImageReference);
  });

Task("Publish")
  .IsDependentOn("Package")
  .Does(() => {
    var settings = new DockerImagePushSettings {
    };
    var imageReference = GetArtifactImageReference();

    DockerPush(settings, imageReference);
  });

Task("Clean")
  .IsDependentOn("CleanCore")
  .Does(() => {
    var settings = new DockerImageRemoveSettings {
      Force = true
    };
    var imageReference = GetArtifactImageReference();

    DockerRemove(settings, imageReference);
  });

RunTarget(target);
