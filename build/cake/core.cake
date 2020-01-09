#addin nuget:?package=Cake.Docker&version=0.11.0

var defaulTarget = "Publish";

var defaultDockerRegistry = "localhost:5000/";
var defaultSampleName = "device-linux";
var defaultSampleVersion = "latest";

var target = Argument("target", defaulTarget);

var dockerRegistry = EnvironmentVariable("DOCKER_REGISTRY", defaultDockerRegistry);
var sampleName = EnvironmentVariable("SAMPLE_NAME", defaultSampleName);
var sampleVersion = EnvironmentVariable("SAMPLE_VERSION", defaultSampleVersion);

var consulHttpAddr = EnvironmentVariable("CONSUL_HTTP_ADDR");

private string GetDockerImageReference() => $"{dockerRegistry}sample-{sampleName}:{sampleVersion}";

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

    if (string.IsNullOrEmpty(consulHttpAddr)) {
      var settings = new DockerComposeUpSettings {
        DetachedMode = true
      };
      var services = new [] { "consul" };
      DockerComposeUp(settings, services);
    }
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
      var logsOutput = logsRunner.RunWithResult("logs", logsSettings, (items) => items.ToArray(), logsService).Last();

      sampleVersion = logsOutput.Split('|')[1].Trim();
      Environment.SetEnvironmentVariable("SAMPLE_VERSION", sampleVersion);
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
