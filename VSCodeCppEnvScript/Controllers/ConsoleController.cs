using Microsoft.Extensions.Logging;
using System;
using CommandLine;
using VSCodeCppEnvScript.Services;
using VSCodeCppEnvScript.Options;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace VSCodeCppEnvScript.Controllers
{
    public class ConsoleController
    {
        private readonly JsonSerializerOptions _serializerOptions
            = new JsonSerializerOptions() { WriteIndented = true };
        private readonly ILogger _logger;
        private readonly IInstallSoftwareService _installSoftwareService;
        private readonly IConfigEnvService _configEnvService;
        private readonly IConfigSysService _configSysService;

        public ConsoleController(
            ILogger logger,
            IInstallSoftwareService installCodeService,
            IConfigEnvService configEnvService,
            IConfigSysService configSysService
        )
        {
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            _installSoftwareService = installCodeService
                ?? throw new ArgumentNullException(nameof(installCodeService));
            _configEnvService = configEnvService
                ?? throw new ArgumentNullException(nameof(configEnvService));
            _configSysService = configSysService
                ?? throw new ArgumentNullException(nameof(configSysService));
        }

        public void ExecCommand(string[] args)
        {
            Parser.Default.ParseArguments<CommandOption>(args)
                .WithParsed(o =>
                {
                    _logger.LogInformation("Parse arguments successed.");
                    Task.WaitAll(
                        _installSoftwareService.InstallSoftware(o.SoftwarePath),
                        _configEnvService.ExtractEnvironment(o.EnvironmentPath),
                        _configEnvService.CreateProjectFolder(o.ProjectPath)
                    );

                    _configSysService.AddToPath(Path.Combine(o.EnvironmentPath, "bin"));
                }).WithNotParsed(e =>
                {
                    _logger.LogError($"Could not parse arguments!\n{JsonSerializer.Serialize(args, _serializerOptions)}");
                    e.Output();
                });
        }
    }
}
