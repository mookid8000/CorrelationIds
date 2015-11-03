using Serilog;

namespace Shared
{
    public static class Logging
    {
        public static void Initialize(string system)
        {
            var configuration = new LoggerConfiguration()
                .Enrich.WithProperty("sub", system)
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile($@"c:\d60\logs\correlationIds\{system}.txt");

            Log.Logger = configuration.CreateLogger();
        }
    }
}
