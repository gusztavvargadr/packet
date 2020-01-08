#addin nuget:?package=Cake.Docker&version=0.11.0

var defaulTarget = "Publish";
var defaultConfiguration = "device-linux";

var defaultDockerRegistry = "localhost:5000/";
var defaultDockerImageTag = "latest";
var defaultConsulHttpAddr = "consul:8500";

var target = Argument("target", defaulTarget);
var configuration = Argument("configuration", defaultConfiguration);

var consulHttpAddr = Argument("consul-http-addr", EnvironmentVariable("CONSUL_HTTP_ADDR", defaultConsulHttpAddr));
var dockerRegistry = Argument("docker-registry", EnvironmentVariable("DOCKER_REGISTRY", defaultDockerRegistry));
var dockerImageTag = Argument("docker-image-tag", EnvironmentVariable("DOCKER_IMAGE_TAG", defaultDockerImageTag));

private string GetDockerImageReference() => $"{dockerRegistry}sample-{configuration}:{dockerImageTag}";

Task("Init")
  .Does(() => {
    StartProcess("docker", "version");
    StartProcess("docker-compose", "version");

    {
      var settings = new DockerComposeBuildSettings {
      };
      var services = new [] { "gitversion" };
      DockerComposeBuild(settings, services);
    }

    if (dockerRegistry == defaultDockerRegistry) {
      var settings = new DockerComposeUpSettings {
        DetachedMode = true
      };
      var services = new [] { "registry" };
      DockerComposeUp(settings, services);
    }

    Environment.SetEnvironmentVariable("DOCKER_REGISTRY", dockerRegistry);

    if (consulHttpAddr == defaultConsulHttpAddr) {
      var settings = new DockerComposeUpSettings {
        DetachedMode = true
      };
      var services = new [] { "consul" };
      DockerComposeUp(settings, services);
    }

    Environment.SetEnvironmentVariable("CONSUL_HTTP_ADDR", consulHttpAddr);

    Environment.SetEnvironmentVariable("SAMPLE_NAME", configuration);
  });

Task("Version")
  .IsDependentOn("Init")
  .Does((context) => {
    if (dockerImageTag == defaultDockerImageTag) {
      var upSettings = new DockerComposeUpSettings {
      };
      var upServices = new [] { "gitversion" };
      DockerComposeUp(upSettings, upServices);

      var logsRunner = new GenericDockerComposeRunner<DockerComposeLogsSettings>(
        context.FileSystem,
        context.Environment,
        context.ProcessRunner,
        context.Tools
      );
      var logsSettings = new DockerComposeLogsSettings {
        NoColor = true
      };
      var logsService = "gitversion";
      var logsOutput = logsRunner.RunWithResult("logs", logsSettings, (items) => items.ToArray(), logsService).Last();

      dockerImageTag = logsOutput.Split('|')[1].Trim();
    }

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
