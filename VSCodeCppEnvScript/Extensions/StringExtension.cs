using System;
using System.IO;

namespace VSCodeCppEnvScript.Extensions
{
    public static class StringExtension
    {
        public static bool IsValidPath(this string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));

            FileInfo file = null;

            try
            {
                file = new FileInfo(path);
            }
            catch (Exception) { }

            return !(file is null);
        }
    }
}
