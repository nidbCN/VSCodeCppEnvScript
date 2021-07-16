using System;
using System.IO;

namespace VSCodeCppEnvScript.Options
{
    public class CommandOption
    {
        public string EnvironmentPath { get; set; }
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "MinGW");
        public string SoftwarePath { get; set; }
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "VSCode");
        public string ProjectPath { get; set; }
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyCodes");
    }
}
