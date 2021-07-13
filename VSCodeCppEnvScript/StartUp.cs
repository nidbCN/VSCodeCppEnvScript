using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace VSCodeCppEnvScript
{
    public class StartUp
    {
        public static IServiceProvider ConfigServices()
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddEventSourceLogger();
                builder.AddConsole();
            });


            return services.BuildServiceProvider();
        }
    }
}
