using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VSCodeCppEnvScript.Utils;

namespace VSCodeCppEnvScript.Services
{
    public class ConfigEnvService : IConfigEnvService
    {
        private readonly string _environmentPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "MinGW");
        private readonly string _zipFilePath =
            Path.Combine(Environment.CurrentDirectory, "mingw.zip");
        private readonly ILogger _logger;

        public ConfigEnvService(ILogger logger)
        {
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> CreateProjectFolder(string path)
        {
            if (!DirectoryUtil.TryCreateFolder(ref path, Path.Combine(_environmentPath, "MinGW")))
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

        public async Task<bool> ExtractEnvironment(string path)
        {
            if (!DirectoryUtil.TryCreateFolder(ref path, Path.Combine(_environmentPath, "MinGW")))
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
    }
}
