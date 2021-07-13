using System.Threading.Tasks;

namespace VSCodeCppEnvScript.Services
{
    public interface IInstallCodeService
    {
        public Task Install(string path);
    }
}
