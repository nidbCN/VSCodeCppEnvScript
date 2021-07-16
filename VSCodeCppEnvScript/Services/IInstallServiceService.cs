using System.Threading.Tasks;

namespace VSCodeCppEnvScript.Services
{
    public interface IInstallServiceService
    {
        public Task InstallSoftware(string path);
    }
}
