using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CommandLine;
using VSCodeCppEnvScript.Services;
using VSCodeCppEnvScript.Options;
using Microsoft.Extensions.Options;

namespace VSCodeCppEnvScript.Controllers
{
    public class ConsoleController
    {
        private readonly JsonSerializerOptions _serializerOptions
            = new JsonSerializerOptions() { WriteIndented = true };
        private readonly ILogger<ConsoleController> _logger;
        private readonly IOptions<MetadataOption> _options;
        private readonly IInstallSoftwareService _installSoftwareService;
        private readonly IConfigEnvService _configEnvService;
        private readonly IConfigSysService _configSysService;

        public ConsoleController(
            ILogger<ConsoleController> logger,
            IOptions<MetadataOption> options,
            IInstallSoftwareService installCodeService,
            IConfigEnvService configEnvService,
            IConfigSysService configSysService
        )
        {
            _options = options 
                ?? throw new ArgumentNullException(nameof(options));
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
                .WithParsed(option =>
                {
                    _logger.LogInformation("Parse arguments successed.");
                    Task.WaitAll(
                        _installSoftwareService.InstallSoftware(option.SoftwarePath),
                        _configEnvService.ExtractEnvironment(option.EnvironmentPath),
                        _configEnvService.CreateProjectFolder(option.ProjectPath)
                    );

                    _configSysService.AddToPath(Path.Combine(option.EnvironmentPath, "bin"));
                })
                .WithNotParsed(error =>
                {
                    _logger.LogError($"Could not parse arguments!\n{JsonSerializer.Serialize(args, _serializerOptions)}");
                    error.Output();
                });
        }
    }
}
