using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VSCodeCppEnvScript.Controllers;
using VSCodeCppEnvScript.Services;
using System.IO;
using VSCodeCppEnvScript.Options;

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

        private static IConfiguration _configuration;

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(config =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json");
                    _configuration = config.Build();
                })
                .ConfigureServices(services =>
                {
                    services.AddLogging(builder =>
                    {
                        builder.AddEventSourceLogger();
                        builder.AddConsole();
                    });

                    services.Configure<MetadataOption>(_configuration);

                    services.AddSingleton<IConfigEnvService, ConfigEnvService>();
                    services.AddSingleton<IConfigSysService, ConfigSysService>();
                    services.AddSingleton<IInstallSoftwareService, InstallSoftwareService>();
                    services.AddSingleton<ConsoleController>();
                });
    }
}
