using System.Threading.Tasks;

namespace VSCodeCppEnvScript.Services
{
    public interface IConfigEnvService
    {
        public Task<bool> ExtractEnvironment(string path);
        public Task<bool> CreateProjectFolder(string path);
    }
}
