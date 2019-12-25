#addin nuget:?package=Cake.Docker&version=0.11.0

var target = Argument("target", "Test");
var configuration = Argument("configuration", "device-linux");

var defaultDockerRegistry = "localhost:5000/gusztavvargadr/packet/";
var dockerRegistry = Argument("docker-registry", EnvironmentVariable("DOCKER_REGISTRY") ?? defaultDockerRegistry);

Task("Init")
  .Does(() => {
    StartProcess("docker", "version");
    StartProcess("docker-compose", "version");
  });

Task("Restore")
  .IsDependentOn("Init")
  .Does(() => {
    if (dockerRegistry == defaultDockerRegistry) {
      var settings = new DockerComposeUpSettings {
        DetachedMode = true
      };
      var services = new [] { "build-registry" };

      DockerComposeUp(settings, services);
    }
  });

Task("Build")
  .IsDependentOn("Restore")
  .Does(() => {
    var settings = new DockerComposeBuildSettings {
    };
    var services = new [] { $"sample-{configuration}" };

    DockerComposeBuild(settings, services);
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    var settings = new DockerComposeRunSettings {
    };
    var service = $"sample-{configuration}";

    var initCommand = "init";
    var initArgs = new [] { "-backend=false" };

    DockerComposeRun(settings, service, initCommand, initArgs);

    var validateCommand = "validate";

    DockerComposeRun(settings, service, validateCommand);
  });

Task("Clean")
  .IsDependentOn("Init")
  .Does(() => {
    var settings = new DockerComposeDownSettings {
      Rmi = "all"
    };

    DockerComposeDown(settings);
  });

RunTarget(target);
