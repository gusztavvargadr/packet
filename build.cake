#addin nuget:?package=Cake.Docker&version=0.11.0

var target = Argument("target", "Publish");
var configuration = Argument("configuration", "sample-device-linux");

var defaultDockerRegistry = "localhost:5000/gusztavvargadr/packet/";
var dockerRegistry = Argument("docker-registry", EnvironmentVariable("DOCKER_REGISTRY", defaultDockerRegistry));

var defaultConsulHttpAddr = "consul:8500";
var consulHttpAddr = Argument("consul-http-addr", EnvironmentVariable("CONSUL_HTTP_ADDR", defaultConsulHttpAddr));

var terraformImageReference = "hashicorp/terraform:0.12.18";

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
      var services = new [] { "registry" };

      DockerComposeUp(settings, services);
    }

    if (consulHttpAddr == defaultConsulHttpAddr) {
      var settings = new DockerComposeUpSettings {
        DetachedMode = true
      };
      var services = new [] { "consul" };

      DockerComposeUp(settings, services);
    }

    {
      var settings = new DockerImagePullSettings {
      };
      var imageReference = terraformImageReference;

      DockerPull(settings, imageReference);
    }
  });

Task("Build")
  .IsDependentOn("Restore")
  .Does(() => {
    var settings = new DockerComposeBuildSettings {
    };
    var services = new [] { configuration };

    DockerComposeBuild(settings, services);
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    var settings = new DockerComposeRunSettings {
    };
    var service = configuration;

    var initCommand = "init";
    var initArgs = new [] { "-backend=false" };

    DockerComposeRun(settings, service, initCommand, initArgs);

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
    var imageReference = $"{dockerRegistry}{configuration}";

    DockerPush(settings, imageReference);
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
