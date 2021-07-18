using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public async Task<bool> CreateProjectFolder()
        {
            _logger.LogInformation("Start config project folders.");

            var path =
                _options.Value.CommandOption.ProjectPath
                ??= _options.Value.DefaultProjectPath;

            if (!DirectoryUtil.TryCreateFolder(ref path, _options.Value.DefaultProjectPath))
            {
                _logger.LogWarning(
                    $"Could not create folder: {path} for project.");
                return false;
            }

            var archieveFileName = _options.Value.EnvArchiveName;

            return await new Task<bool>(() =>
            {
                _logger.LogInformation(
                    $"Start extract project workspace {archieveFileName} to {path}.");

                // Extarct file.
                try
                {
                    ZipFile.ExtractToDirectory(
                        archieveFileName,
                        path);
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

                // Use path string to replace %var% in config files.
                var files = new string[]
                {
                    Path.Combine(archieveFileName, "C-Codes", @"c_cpp_properties.json"),
                    Path.Combine(archieveFileName, "C-Codes", @"launch.json"),
                    Path.Combine(archieveFileName, "Cpp-Codes", @"c_cpp_properties.json"),
                    Path.Combine(archieveFileName, "Cpp-Codes", @"launch.json"),
                };

                var rulesDict = new Dictionary<string, string>()
                {
                    { "%MIGW_PATH%", path}
                };

                if (!new ConfigFileFilter(rulesDict).TryFilterFiles(files))
                {
                    _logger.LogWarning("Some workspace config files cannot be found when filter files!");
                }

                // update path.
                if (_options.Value.CommandOption.ProjectPath != path)
                {
                    _options.Value.CommandOption.ProjectPath = path;
                }

                _logger.LogInformation("Success config project workspace.");

                return true;
            });
        }

        public async Task<bool> ExtractEnvironment()
        {
            _logger.LogInformation("Start config environment.");

            var path =
                _options.Value.CommandOption.EnvironmentPath
                ??= _options.Value.DefaultEnvironmentPath;

            if (!DirectoryUtil.TryCreateFolder(ref path, _options.Value.DefaultEnvironmentPath))
            {
                _logger.LogWarning($"Could not create folder: {path} for environment.");
                return false;
            }

            return await new Task<bool>(() =>
            {
                var archieveFileName = _options.Value.EnvArchiveName;

                _logger.LogInformation(
                    $"Start extract project workspace {archieveFileName} to {path}.");

                try
                {
                    ZipFile.ExtractToDirectory(archieveFileName, path);
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

                if (path != _options.Value.CommandOption.EnvironmentPath)
                {
                    _options.Value.CommandOption.EnvironmentPath = path;
                }

                _logger.LogInformation("Success install environment.");

                return true;
            });
        }
    }
}
