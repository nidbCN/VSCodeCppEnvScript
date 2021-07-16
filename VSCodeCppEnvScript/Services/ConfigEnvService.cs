using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VSCodeCppEnvScript.Services
{
    public class ConfigEnvService : IConfigEnvService
    {
        private readonly string _defalutPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private readonly string _zipFilePath =
            Path.Combine(Environment.CurrentDirectory, "mingw.zip");
        private readonly ILogger _logger;

        public ConfigEnvService(ILogger logger)
        {
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<bool> CreateProjectFolder(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExtractEnvironment(string path)
        {
            if (!TryCreateFolder(path, "MinGW"))
                return new Task<bool>(() => false).Result;

            return await new Task<bool>(() =>
            {
                try
                {
                    ZipFile.ExtractToDirectory(_zipFilePath, path);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unzip package error!\n{ex.Message}");
                    return false;
                }
            });
        }

        private bool TryCreateFolder(string path, string defaultFolderName)
        {
            var result = true;
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                _logger.LogError($"Could not create folder at {path}, try default path.\n{e.Message}");
                var backupPath = Path.Combine(_defalutPath, defaultFolderName);
                try
                {
                    Directory.CreateDirectory(backupPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Could not create folder at {backupPath}, fault!\n{ex.Message}");
                    result = false;
                }
            }

            return result;
        }
    }
}
