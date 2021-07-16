using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using VSCodeCppEnvScript.Extensions;

namespace VSCodeCppEnvScript.Services
{
    public class ConfigEnvService : IConfigEnvService
    {
        private readonly string _defalutPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        private readonly string _zipFilePath =
            Path.Combine(Environment.CurrentDirectory, "mingw.zip");

        public ConfigEnvService()
        {

        }

        public Task<bool> CreateProjectFolder(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExtractEnvironment(string path)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.TryCreate())
            {
                path = _defalutPath;
                dir = new DirectoryInfo(path);

                if (!dir.TryCreate())
                    return await new Task<bool>(() => false);
            }

            return await new Task<bool>(() =>
            {
                try
                {
                    ZipFile.ExtractToDirectory(_zipFilePath, path);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
    }
}
