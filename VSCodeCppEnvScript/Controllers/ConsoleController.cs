using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VSCodeCppEnvScript.Services;

namespace VSCodeCppEnvScript.Controllers
{
    public class ConsoleController
    {
        private readonly string _defaultPath = 
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private readonly ILogger _logger;
        private readonly IInstallCodeService _installCodeService;
        private readonly IConfigEnvService _configEnvService;
        private readonly IConfigSysService _configSysService;

        public ConsoleController(
            ILogger logger,
            IInstallCodeService installCodeService,
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

        public async void ExecCommand(string[] args) 
        {
            var codePath = Path.Combine(_defaultPath, "VSCode");
            var envPath = Path.Combine(_defaultPath, "MinGW-w64");

            if (args.Length == 0)
            {

            }

            foreach(var argStr in args)
            {
                if (!argStr.StartsWith('-') && !argStr.StartsWith('/'))
                {
                    _logger.LogError($"Invalid argument:{argStr}, skip.");
                    ShowHelpMessage();
                    continue;
                }

                var argArray = argStr[1..].ToLower().Split('=');


                switch(argArray[0])
                {
                    case "h":
                    case "-help":
                        ShowHelpMessage();
                        break;
                    case "ep":
                    case "-envPath":
                }
            }
        }

        private void ShowHelpMessage()
        {
            Console.WriteLine("-h | --help: Show help message.");
            Console.WriteLine("-cp=<path> | --codePath=<pah>: VS Code install path.");
            Console.WriteLine("-ep=<path> | --envPath=<path>: Environment path.");
        }
    }
}
