using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VSCodeCppEnvScript.Utils;
using VSCodeCppEnvScript.Options;

namespace VSCodeCppEnvScript.Services
{
    public class InstallSoftwareService : IInstallSoftwareService
    {
        private readonly IOptions<MetadataOption> _options;
        private readonly ILogger<InstallSoftwareService> _logger;

        public InstallSoftwareService(ILogger<InstallSoftwareService> logger, IOptions<MetadataOption> options)
        {
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
            _options = options 
                ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task InstallSoftware()
        {
            _logger.LogInformation("Start install VSCode.");

            // Create folder.
            var path = 
                _options.Value.CommandOption.SoftwarePath 
                ??= _options.Value.DefaultSoftwarePath;

            if (!DirectoryUtil.TryCreateFolder(ref path, _options.Value.DefaultSoftwarePath))
            {
                _logger.LogWarning(
                    $"Could not create folder: {path} for VSCode.");
                return;
            }

            _logger.LogInformation("Successed create folder.\n" + path);

            // Install code.
            var installArgPath = $"/DIR=\"{path}\"";
            const string installArgTask = "/MERGETASKS=\"!runcode,desktopicon,quicklaunchicon,addcontextmenufiles,addcontextmenufolders,associatewithfiles,addtopath\"";

            _logger.LogInformation($"Install VSCode to {path}");

            var installer = new Process()
            {
                StartInfo =
                {
                    FileName = "",
                    CreateNoWindow = true,
                    ArgumentList =
                    {
                        "/SP-", "/VERYSILENT", "/CLOSEAPPLICATIONS", "/NOCANCEL" ,
                        "/NORESTART", "/CLOSEAPPLICATIONS", "/ALLUSERS",installArgPath,
                        "/TYPE=FULL", installArgTask,
                    }
                }
            };

            await new Task(() =>
            {
                _logger.LogInformation("Run VSCode install program.\n" + installer.StartInfo.Arguments);
                installer.Start();
                installer.WaitForExit();

                if(installer.ExitCode == 0)
                {
                    _logger.LogInformation("Successed install VSCode.");
                }
                else
                {
                    _logger.LogError("Failed install VSCode!");
                }

            });

            // Update path.
            if (_options.Value.CommandOption.SoftwarePath != path)
            {
                _options.Value.CommandOption.SoftwarePath = path;
            }
        }
    }
}
