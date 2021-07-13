using System.IO;

namespace VSCodeCppEnvScript.Services
{
    public class ConfigSysService : IConfigSysService
    {
        public void ConfigEnvVariables(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
