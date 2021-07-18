using System.Threading.Tasks;

namespace VSCodeCppEnvScript.Services
{
    public interface IConfigEnvService
    {
        public Task<bool> ExtractEnvironment();
        public Task<bool> CreateProjectFolder();
    }
}
