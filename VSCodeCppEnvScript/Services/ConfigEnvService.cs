using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
            _options.Value.CodeInstallerName = "TEST";

            return false;


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

                var files = new string[]
                {
                    Path.Combine(archieveFileName, "C-Codes", @"c_cpp_properties.json"),
                    Path.Combine(archieveFileName, "C-Codes", @"launch.json"),
                    Path.Combine(archieveFileName, "Cpp-Codes", @"c_cpp_properties.json"),
                    Path.Combine(archieveFileName, "Cpp-Codes", @"launch.json"),
                };

                var rulesDict = new Dictionary<string, string>() 
                {
                    { "%MIGW_PATH%", ""}
                };



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

        private static void ParseConfigFile(string path, Dictionary<string, string> dict)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));

            var content = File.ReadAllText(path);

            foreach (var key in dict.Keys)
            {
                if (dict.TryGetValue(key, out string dest))
                {
                    content = content.Replace(key, dest);
                }
            }

            File.WriteAllText(path, content);

        }

    }
}
