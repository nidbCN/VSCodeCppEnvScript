using System.Threading.Tasks;

namespace VSCodeCppEnvScript.Services
{
    public interface IConfigEnvService
    {
        public Task<bool> ExtractToPath(string path);
    }
}
