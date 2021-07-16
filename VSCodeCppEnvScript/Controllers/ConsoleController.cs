using Microsoft.Extensions.Logging;
using System;
using CommandLine;
using VSCodeCppEnvScript.Services;
using VSCodeCppEnvScript.Options;

namespace VSCodeCppEnvScript.Controllers
{
    public class ConsoleController
    {
        private readonly ILogger _logger;
        private readonly IInstallServiceService _installCodeService;
        private readonly IConfigEnvService _configEnvService;
        private readonly IConfigSysService _configSysService;

        public ConsoleController(
            ILogger logger,
            IInstallServiceService installCodeService,
            IConfigEnvService configEnvService,
            IConfigSysService configSysService
        )
        {
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            _installCodeService = installCodeService
                ?? throw new ArgumentNullException(nameof(installCodeService));
            _configEnvService = configEnvService
                ?? throw new ArgumentNullException(nameof(configEnvService));
            _configSysService = configSysService
                ?? throw new ArgumentNullException(nameof(configSysService));
        }

        public void ExecCommand(string[] args)
        {
            Parser.Default.ParseArguments<CommandOption>(args)
                .WithParsed(async o =>
                {
                    await _installCodeService.InstallSoftware(o.SoftwarePath);
                    await _configEnvService.ExtractEnvironment(o.EnvironmentPath);
                    await _configEnvService.CreateProjectFolder(o.ProjectPath);
                    _configSysService.AddToPath(o.EnvironmentPath);

                }).WithNotParsed(e =>
                {

                });

        }

    }
}
