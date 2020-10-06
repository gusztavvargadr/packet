#load ./build/cake/core.cake

Task("Restore")
  .IsDependentOn("RestoreCore")
  .Does(() => {
    var sampleImageReference = GetSampleImageReference();
    var artifactImageReference = GetArtifactImageReference();

    {
      var settings = new DockerImagePullSettings {
      };

      DockerPull(settings, artifactImageReference);
    }

    {
      DockerTag(artifactImageReference, sampleImageReference);
    }

    {
      var settings = new DockerComposeRunSettings {
      };
      var service = "sample";
      var command = "init";

      DockerComposeRun(settings, service, command);
    }
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
  });

Task("Package")
  .IsDependentOn("Test")
  .Does(() => {
    var sampleImageReference = GetSampleImageReference();
    var deployImageReference = GetDeployImageReference();

    DockerTag(sampleImageReference, deployImageReference);
  });

Task("Publish")
  .IsDependentOn("Package")
  .Does(() => {
    var settings = new DockerImagePushSettings {
    };
    var imageReference = GetDeployImageReference();

    DockerPush(settings, imageReference);
  });

Task("Clean")
  .IsDependentOn("CleanCore")
  .Does(() => {
    var settings = new DockerImageRemoveSettings {
      Force = true
    };
    var artifactImageReference = GetArtifactImageReference();
    var deployImageReference = GetDeployImageReference();

    DockerRemove(settings, artifactImageReference, deployImageReference);
  });

RunTarget(target);
