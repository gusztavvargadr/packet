#addin nuget:?package=Cake.Docker&version=0.11.0

var target = Argument("target", "Test");
var configuration = Argument("configuration", "sample-device-linux");

var dockerRegistry = Argument("docker-registry", EnvironmentVariable("DOCKER_REGISTRY"));

var terraformImageReference = "hashicorp/terraform:0.12.18";

Task("Init")
  .Does(() => {
    StartProcess("docker", "version");
    StartProcess("docker-compose", "version");
  });

Task("Restore")
  .IsDependentOn("Init")
  .Does(() => {
    var settings = new DockerImagePullSettings {
    };
    var imageReference = terraformImageReference;

    DockerPull(settings, imageReference);
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

Task("Clean")
  .IsDependentOn("Init")
  .Does(() => {
    var settings = new DockerComposeDownSettings {
      Rmi = "all"
    };

    DockerComposeDown(settings);
  });

RunTarget(target);
