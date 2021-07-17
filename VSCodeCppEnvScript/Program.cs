using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using VSCodeCppEnvScript.Controllers;
using VSCodeCppEnvScript.Services;

namespace VSCodeCppEnvScript
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            var controller = host.Services.GetRequiredService<ConsoleController>();
            controller.ExecCommand(args);
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLogging(builder =>
                    {
                        builder.AddEventSourceLogger();
                        builder.AddConsole();
                    });

                    services.AddSingleton<IConfigEnvService, ConfigEnvService>();
                    services.AddSingleton<IConfigSysService, ConfigSysService>();
                    services.AddSingleton<IInstallSoftwareService, InstallSoftwareService>();
                    services.AddSingleton<ConsoleController>();
                });
    }
}
