using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace VSCodeCppEnvScript.Utils
{
    public static class DirectoryUtil
    {
        public static bool TryCreateFolder(ref string path, string defaultPath)
        {
            var result = true;
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception)
            {
                try
                {
                    Directory.CreateDirectory(defaultPath);
                    path = defaultPath;
                }
                catch (Exception)
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
