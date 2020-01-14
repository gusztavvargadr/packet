#addin nuget:?package=Cake.Docker&version=0.11.0

var defaulTarget = "Publish";

var target = Argument("target", defaulTarget);

var defaultConsulHttpAddr = "consul:8500";

var defaultDockerRegistry = "localhost:5000/";
var defaultSampleName = "device-linux";
var defaultSampleVersion = "latest";

var consulHttpAddr = EnvironmentVariable("CONSUL_HTTP_ADDR", defaultConsulHttpAddr);

var dockerRegistry = EnvironmentVariable("DOCKER_REGISTRY", defaultDockerRegistry);
var sampleName = EnvironmentVariable("SAMPLE_NAME", defaultSampleName);
var sampleVersion = EnvironmentVariable("SAMPLE_VERSION", defaultSampleVersion);

private string GetDockerImageReference() => $"{dockerRegistry}sample-{sampleName}:{sampleVersion}";

Task("Init")
  .Does(() => {
    StartProcess("docker", "version");
    StartProcess("docker-compose", "version");

    var settings = new DockerComposeBuildSettings {
    };
    var services = new [] { "gitversion" };
    DockerComposeBuild(settings, services);
  });

Task("Version")
  .IsDependentOn("Init")
  .Does((context) => {
    if (sampleVersion == defaultSampleVersion) {
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
      var logsOutput = logsRunner.RunWithResult(
        "logs",
        logsSettings,
        (items) => items.Where(item => item.Contains('|')).ToArray(),
        logsService
      ).Last();

      sampleVersion = logsOutput.Split('|')[1].Trim();
      Environment.SetEnvironmentVariable("SAMPLE_VERSION", sampleVersion);
    }
    Information($"Sample version: '{sampleVersion}'.");
  });

Task("RestoreCore")
  .IsDependentOn("Version")
  .Does(() => {
    {
      var settings = new DockerComposeBuildSettings {
      };
      var services = new [] { "registry", "consul" };
      DockerComposeBuild(settings, services);
    }

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
  });

Task("Clean")
  .IsDependentOn("Version")
  .Does(() => {
    var settings = new DockerComposeDownSettings {
      Rmi = "all"
    };
    DockerComposeDown(settings);
  });
