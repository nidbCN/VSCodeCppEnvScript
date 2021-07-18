using System;
using System.IO;
using CommandLine;

namespace VSCodeCppEnvScript.Options
{
    public class MetadataOption
    {
        [Option('e', "environmentPath", Required = false, HelpText = "Set the path to install MinGW.")]
        public string EnvironmentPath { get; set; }

        [Option('s', "softwarePath", Required = false, HelpText = "Set the path to install VSCode.")]
        public string SoftwarePath { get; set; }

        [Option('p', "projectPath", Required = false, HelpText = "Set the path of VSCode workspace.")]
        public string ProjectPath { get; set; }

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
