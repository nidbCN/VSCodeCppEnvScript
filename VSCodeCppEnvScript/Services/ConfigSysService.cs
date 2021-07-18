using System;
using Microsoft.Extensions.Logging;
using VSCodeCppEnvScript.Utils;

namespace VSCodeCppEnvScript.Services
{
    public class ConfigSysService : IConfigSysService
    {
        private readonly ILogger<ConfigSysService> _logger;

        public ConfigSysService(ILogger<ConfigSysService> logger)
        {
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public void AddToPath(params string[] paths)
        {
            _logger.LogInformation("Start add environment bin to path.");

            EnvironmentUtil.AddToPath(paths);

            _logger.LogInformation($"Success add to path.");
        }
    }
}
