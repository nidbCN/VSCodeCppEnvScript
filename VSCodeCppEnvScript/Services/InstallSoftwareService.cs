using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VSCodeCppEnvScript.Utils;

namespace VSCodeCppEnvScript.Services
{
    public class InstallSoftwareService : IInstallSoftwareService
    {
        private readonly string _defalutPath 
            = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private readonly ILogger _logger;

        public InstallSoftwareService(ILogger logger)
        {
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InstallSoftware(string path)
        {
            if(path is null) throw new ArgumentNullException(nameof(path));

            if (!DirectoryUtil.TryCreateFolder(ref path, Path.Combine(_defalutPath, "VSCode")))
                return;

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
                installer.Start();
                installer.WaitForExit();
            });
        }
    }
}
