using System;
using System.IO;

namespace VSCodeCppEnvScript.Extensions
{
    public static class DirectoryExtension
    {
        public static bool TryCreate(this DirectoryInfo directory)
        {
            try
            {
                if (directory.Exists)
                {
                    directory.Create();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
