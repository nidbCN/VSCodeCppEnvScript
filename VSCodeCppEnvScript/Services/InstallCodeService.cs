using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using VSCodeCppEnvScript.Extensions;

namespace VSCodeCppEnvScript.Services
{
    public class InstallCodeService : IInstallCodeService
    {
        private readonly string _defalutPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        public InstallCodeService()
        {

        }

        public async Task Install(string path)
        {
            if(path is null) throw new ArgumentNullException(nameof(path));       

            var dirInfo = new DirectoryInfo(path);

            path = dirInfo.TryCreate() ? dirInfo.FullName : _defalutPath;

            var installArgPath = $"/DIR=\"{path}\"";
            const string installArgTask = "/MERGETASKS=\"!runcode,desktopicon,quicklaunchicon,addcontextmenufiles,addcontextmenufolders,associatewithfiles,addtopath\"";

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
