using System;
using System.IO;

namespace VSCodeCppEnvScript.Extensions
{
    public static class StringExtension
    {
        public static bool IsValidPath(this string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));

            try
            {
                _ = new FileInfo(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
