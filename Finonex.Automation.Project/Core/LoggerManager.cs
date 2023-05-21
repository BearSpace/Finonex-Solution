using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace Finonex.Automation.Project.Core;

public static class LoggerManager
{
    public static Logger CreateLoggerInstance()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        return logger;
    }
}