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
    var upSettings = new DockerComposeUpSettings {
    };
    var upServices = new [] { "gitversion" };

    DockerComposeUp(upSettings, upServices);

    var runner = new GenericDockerComposeRunner<DockerComposeLogsSettings>(
      context.FileSystem,
      context.Environment,
      context.ProcessRunner,
      context.Tools
    );

    var logsSettings = new DockerComposeLogsSettings {
      NoColor = true
    };
    var logsService = "gitversion";

    dockerImageTag = string.Join(
      Environment.NewLine,
      runner.RunWithResult("logs", logsSettings, (items) => items.ToArray(), logsService)
    ).Split('|')[1].Trim();
    Information(dockerImageTag);

    Environment.SetEnvironmentVariable("DOCKER_IMAGE_TAG", dockerImageTag);
  });

Task("Clean")
  .IsDependentOn("Version")
  .Does(() => {
    var settings = new DockerComposeDownSettings {
      Rmi = "all"
    };

    DockerComposeDown(settings);
  });
