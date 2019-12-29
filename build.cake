#addin nuget:?package=Cake.Docker&version=0.11.0

var target = Argument("target", "Publish");
var configuration = Argument("configuration", "device-linux");

var defaultDockerRegistry = "localhost:5000/";
var dockerRegistry = Argument("docker-registry", EnvironmentVariable("DOCKER_REGISTRY", defaultDockerRegistry));

var defaultDockerImageTag = "latest";
var dockerImageTag = Argument("docker-image-tag", EnvironmentVariable("DOCKER_IMAGE_TAG", defaultDockerImageTag));

var defaultConsulHttpAddr = "consul:8500";
var consulHttpAddr = Argument("consul-http-addr", EnvironmentVariable("CONSUL_HTTP_ADDR", defaultConsulHttpAddr));

Task("Init")
  .Does(() => {
    StartProcess("docker", "version");
    StartProcess("docker-compose", "version");

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

    Environment.SetEnvironmentVariable("SAMPLE_NAME", configuration);
  });

Task("Version")
  .IsDependentOn("Init")
  .Does((context) => {
    var buildSettings = new DockerComposeBuildSettings {
    };
    var buildServices = new [] { "gitversion" };

    DockerComposeBuild(buildSettings, buildServices);

    var runSettings = new DockerComposeRunSettings {
    };
    var runService = "gitversion";
    var runCommand = "/output json";

    DockerComposeRun(runSettings, runService, runCommand);

    runCommand = "/showvariable SemVer";

    var runner = new GenericDockerComposeRunner<DockerComposeRunSettings>(
      context.FileSystem,
      context.Environment,
      context.ProcessRunner,
      context.Tools
    );
    var result = string.Join(
      Environment.NewLine,
      runner.RunWithResult("run", runSettings, (items) => items.ToArray(), runService, runCommand)
    );

    dockerImageTag = result.Substring(7);
    Information(dockerImageTag);

    Environment.SetEnvironmentVariable("DOCKER_IMAGE_TAG", dockerImageTag);
  });

Task("Restore")
  .IsDependentOn("Version")
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
    var settings = new DockerComposeRunSettings {
    };
    var service = "sample";

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
    var imageReference = $"{dockerRegistry}gusztavvargadr/packet/samples/{configuration}:{dockerImageTag}";
    Information($"{imageReference}.");

    DockerPush(settings, imageReference);
  });

Task("Clean")
  .IsDependentOn("Version")
  .Does(() => {
    var settings = new DockerComposeDownSettings {
      Rmi = "all"
    };

    DockerComposeDown(settings);
  });

RunTarget(target);
