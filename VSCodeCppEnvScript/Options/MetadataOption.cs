using System;
using System.IO;
using CommandLine;

namespace VSCodeCppEnvScript.Options
{
    public class MetadataOption
    {
        public CommandOption CommandOption { get; set; } = new CommandOption();

        public string DefaultEnvironmentPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "MinGW");
        public string DefaultSoftwarePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "VSCode");
        public string DefaultProjectPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyCodes");

        public string EnvArchiveName { get; set; }
        public string ProjConfigArchieveName { get; set; }
        public string CodeInstallerName { get; set; }
    }
}
