#load ./build/cake/core.cake

var dockerRegistryPublish = EnvironmentVariable("DOCKER_REGISTRY_PUBLISH");

Task("Restore")
  .IsDependentOn("RestoreCore")
  .Does(() => {
    var imageReference = GetDockerImageReference();

    {
      var settings = new DockerImagePullSettings {
      };
      DockerPull(settings, imageReference);
    }

    if (!string.IsNullOrEmpty(dockerRegistryPublish)) {
      dockerRegistry = dockerRegistryPublish;
      Environment.SetEnvironmentVariable("DOCKER_REGISTRY", dockerRegistry);
      var registryReference = GetDockerImageReference();
      DockerTag(imageReference, registryReference);

      {
        var settings = new DockerImagePushSettings {
        };
        DockerPush(settings, registryReference);
      }

      {
        var settings = new DockerImagePullSettings {
        };
        DockerPull(settings, registryReference);
      }
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
  });

Task("Publish")
  .IsDependentOn("Package")
  .Does(() => {
  });

RunTarget(target);
