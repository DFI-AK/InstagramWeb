using Serilog;
using Serilog.Events;

namespace InstagramWeb.Web.Infrastructure;

public static class ApplicationLogExtension
{
    public static IServiceCollection AddApplicationLog(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.WithProperty("Application", "Instagram")
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();

        services.AddLogging(o => o.AddSerilog(Log.Logger, dispose: true));

        return services;
    }
}
