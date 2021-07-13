using System;
using System.Linq;

namespace VSCodeCppEnvScript.Services
{
    public class ConfigSysService : IConfigSysService
    {
        public ConfigSysService()
        {

        }

        public void AddToPath(params string[] paths)
        {
            var envPaths = Environment.GetEnvironmentVariable(
                "path",
                EnvironmentVariableTarget.Machine
            );

            var envPathsAddList = 
                envPaths.Split(';')
                .Where((x, i) => x != paths[i])
                .ToList();

            var envPathsAppend = string.Join(';', envPathsAddList);

            Environment.SetEnvironmentVariable(
                "path",
                envPathsAppend + envPaths,
                EnvironmentVariableTarget.Machine
            );
        }
    }
}
