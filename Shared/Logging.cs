using Rebus.Serilog;
using Serilog;

namespace Shared
{
    public static class Logging
    {
        public static void Initialize(string system)
        {
            var configuration = new LoggerConfiguration()
                .Enrich.WithRebusCorrelationId("CorrelationId")
                .Enrich.WithProperty("sub", system)
                .WriteTo.ColoredConsole(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}][{CorrelationId}] {Message}{NewLine}{Exception}")
                .Enrich.With<HttpContextCorrelationIdEnricher>()
                .WriteTo.RollingFile($@"c:\d60\logs\correlationIds\{system}.txt");

            Log.Logger = configuration.CreateLogger();
        }
    }
}
