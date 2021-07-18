using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VSCodeCppEnvScript.Options;
using VSCodeCppEnvScript.Utils;

namespace VSCodeCppEnvScript.Services
{
    public class ConfigEnvService : IConfigEnvService
    {
        private readonly ILogger<ConfigEnvService> _logger;
        private readonly IOptions<MetadataOption> _options;

        public ConfigEnvService(ILogger<ConfigEnvService> logger, IOptions<MetadataOption> options)
        {
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            _options = options
                ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<bool> CreateProjectFolder(string path)
        {
            if (!DirectoryUtil.TryCreateFolder(ref path, _options.Value.DefaultProjectPath))
                return new Task<bool>(() => false).Result;

            var archieveFileName = _options.Value.EnvArchiveName;
            var defaultProjPath = _options.Value.DefaultProjectPath;

            return await new Task<bool>(() =>
            {
                try
                {
                    ZipFile.ExtractToDirectory(
                        archieveFileName,
                        defaultProjPath);
                }
                catch (Exception ex) when (ex is NotSupportedException || ex is InvalidDataException)
                {
                    _logger.LogError($"Archieve file {archieveFileName} is damaged, please check your download!\n{ex.Message}");
                    return false;
                }
                catch (Exception ex) when (ex is FileNotFoundException)
                {
                    _logger.LogError($"Archieve file {archieveFileName} has losed, please check your download!\n{ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Could not unzip file, unknow error!\n{ex.Message}");
                    return false;
                }

                var cDir = Path.Combine(defaultProjPath, "C-Codes");
                var cppDir = Path.Combine(defaultProjPath, "Cpp-Codes");



                return true;
            });
        }

        public async Task<bool> ExtractEnvironment(string path)
        {
            if (!DirectoryUtil.TryCreateFolder(ref path, _options.Value.DefaultEnvironmentPath))
                return new Task<bool>(() => false).Result;

            return await new Task<bool>(() =>
            {
                try
                {
                    ZipFile.ExtractToDirectory(_options.Value.EnvArchiveName, path);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unzip package error!\n{ex.Message}");
                    return false;
                }
            });
        }
    }
}
