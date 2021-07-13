using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VSCodeCppEnvScript.Services
{
    public class InstallCodeService : IInstallCodeService
    {
        private const string _defalutPath = "";

        public async Task Install(string path = _defalutPath)
        {
            var installArgPath = $"/DIR=\"{path}\"";
            var installArgTask = "/MERGETASKS=\"!runcode,desktopicon,quicklaunchicon,addcontextmenufiles,addcontextmenufolders,associatewithfiles,addtopath\"";

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
