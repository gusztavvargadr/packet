#addin nuget:?package=Cake.Docker&version=0.11.0
#addin nuget:?package=Cake.SemVer&version=4.0.0
#addin nuget:?package=semver&version=2.0.4

var target = Argument("target", "Publish");
var sampleName = Argument("sample-name", "device-linux");

var sourceVersion = Argument("source-version", string.Empty);
Semver.SemVersion sourceSemVer;
var buildVersion = Argument("build-version", string.Empty);
var projectVersion = Argument("project-version", string.Empty);
var packageVersion = Argument("package-version", string.Empty);

var defaultDockerRegistry = "localhost:5000/";
var dockerRegistry = EnvironmentVariable("DOCKER_REGISTRY", defaultDockerRegistry);
var defaultConsulHttpAddr = "consul:8500";
var consulHttpAddr = EnvironmentVariable("CONSUL_HTTP_ADDR", defaultConsulHttpAddr);

var sourceRegistry = Argument("source-registry", string.Empty);
if (string.IsNullOrEmpty(sourceRegistry)) {
  sourceRegistry = dockerRegistry;
}
var packageRegistry = Argument("package-registry", string.Empty);
if (string.IsNullOrEmpty(packageRegistry)) {
  packageRegistry = dockerRegistry;
}

private string GetSampleImageReference() => EnvironmentVariable("SAMPLE_IMAGE");

Task("Init")
  .Does(() => {
    StartProcess("docker", "--version");
    StartProcess("docker-compose", "--version");

    Environment.SetEnvironmentVariable("SAMPLE_NAME", sampleName);
    Information($"SAMPLE_NAME: '{sampleName}'.");
  });

Task("Version")
  .IsDependentOn("Init")
  .Does((context) => {
    if (string.IsNullOrEmpty(sourceVersion)) {
      {
        var settings = new DockerComposeUpSettings {
        };
        var services = new [] { "gitversion" };
        DockerComposeUp(settings, services);
      }

      {
        var runner = new GenericDockerComposeRunner<DockerComposeLogsSettings>(
          context.FileSystem,
          context.Environment,
          context.ProcessRunner,
          context.Tools
        );
        var settings = new DockerComposeLogsSettings {
          NoColor = true
        };
        var service = "gitversion";
        var output = runner.RunWithResult(
          "logs",
          settings,
          (items) => items.Where(item => item.Contains('|')).ToArray(),
          service
        ).Last();

        sourceVersion = output.Split('|')[1].Trim();
      }
    }
    Information($"Source version: '{sourceVersion}'.");
    sourceSemVer = ParseSemVer(sourceVersion);

    if (string.IsNullOrEmpty(buildVersion)) {
      buildVersion = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
    }
    Information($"Build version: '{buildVersion}'.");

    if (string.IsNullOrEmpty(projectVersion)) {
      projectVersion = sourceVersion;
    }
    Information($"Project version: '{projectVersion}'.");

    if (string.IsNullOrEmpty(packageVersion)) {
      packageVersion = sourceVersion;
    }
    Information($"Package version: '{packageVersion}'.");

    var sampleImage = $"{sourceRegistry}samples-{sampleName}:{sourceVersion}";
    Environment.SetEnvironmentVariable("SAMPLE_IMAGE", sampleImage);
    Information($"SAMPLE_IMAGE: '{sampleImage}'.");
  });

Task("RestoreCore")
  .IsDependentOn("Version")
  .Does(() => {
    if (sourceRegistry == defaultDockerRegistry) {
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
