using System;
using Rebus.Serilog;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Shared
{
    public static class Logging
    {
        const string Template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}][{CorrelationId}] {Message}{NewLine}{Exception}";

        public static void Initialize(string system)
        {
            var configuration = new LoggerConfiguration()
                .Enrich.WithRebusCorrelationId("CorrelationId")
                .Enrich.WithProperty("app", "devops")
                .Enrich.WithProperty("sub", system)
                .Enrich.With<HttpContextCorrelationIdEnricher>()
                .Enrich.With<EmptyCorrelationIdIfNecessary>()
                .WriteTo.RollingFile($@"c:\d60\logs\correlationIds\{system}.txt", outputTemplate: Template)
                .WriteTo.ColoredConsole(outputTemplate: Template)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("https://devops:HejMedDigMinVen@nsa.d60.dk"))
                {
                    BufferBaseFilename = $@"c:\d60\logs\correlationIds\es\{system}.txt"
                });

            Log.Logger = configuration.CreateLogger();
        }
    }

    public class EmptyCorrelationIdIfNecessary : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", ""));
        }
    }
}
