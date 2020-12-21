using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNetHostEnvironment
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                                   {
                                       services.AddHostedService<App>();
                                   });
    }

    public class App : IHostedService
    {
        private readonly ILogger<App> _logger;

        private readonly IHostApplicationLifetime _appLifetime;

        private readonly IHostEnvironment _env;

        private readonly IConfiguration _configuration;

        public App(ILogger<App>             logger,
                   IHostApplicationLifetime appLifetime,
                   IHostEnvironment         env,
                   IConfiguration           configuration)
        {
            _logger        = logger;
            _appLifetime   = appLifetime;
            _env           = env;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("App running at: {time}",                  DateTimeOffset.Now);
            _logger.LogInformation("App running at Env: {env}",               _env.EnvironmentName);
            _logger.LogInformation("App running at Configuration Key: {key}", _configuration.GetSection("key").Value);

            await Task.Yield();

            _appLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("App stopped at: {time}", DateTimeOffset.Now);
            return Task.CompletedTask;
        }
    }
}
