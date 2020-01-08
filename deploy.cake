#load ./build/cake/core.cake

var dockerRegistryPublish = Argument("docker-registry-publish", EnvironmentVariable("DOCKER_REGISTRY_PUBLISH", defaultDockerRegistry));

Task("Restore")
  .IsDependentOn("Version")
  .Does(() => {
    var settings = new DockerImagePullSettings {
    };
    var imageReference = GetDockerImageReference();
    DockerPull(settings, imageReference);

    dockerRegistry = dockerRegistryPublish;
    Environment.SetEnvironmentVariable("DOCKER_REGISTRY", dockerRegistry);
    var registryReference = GetDockerImageReference();
    DockerTag(imageReference, registryReference);
  });

Task("Build")
  .IsDependentOn("Restore")
  .Does(() => {
    {
      var settings = new DockerComposeRunSettings {
      };
      var service = "sample";
      var command = "init";
      DockerComposeRun(settings, service, command);
    }
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
    {
      var settings = new DockerImagePushSettings {
      };
      var imageReference = GetDockerImageReference();
      DockerPush(settings, imageReference);
    }

    {
      var settings = new DockerImageRemoveSettings {
      };
      var images = new [] { GetDockerImageReference() };
      DockerRemove(settings, images);
    }
  });

RunTarget(target);
