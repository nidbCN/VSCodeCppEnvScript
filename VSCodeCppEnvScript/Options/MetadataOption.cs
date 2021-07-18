using System;
using System.IO;

namespace VSCodeCppEnvScript.Options
{
    public class MetadataOption
    {
        public string[] ConsoleArgs { get; set; }
        public string EnvArchiveName { get; set; }
        public string ProjConfigArchieveName { get; set; }
        public string CodeInstallerName { get; set; }
        public string DefaultEnvironmentPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "MinGW");
        public string DefaultSoftwarePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "VSCode");
        public string DefaultProjectPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyCodes");
    }
}
