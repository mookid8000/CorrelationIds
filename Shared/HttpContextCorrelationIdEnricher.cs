using System.Web;
using Serilog.Core;
using Serilog.Events;

namespace Shared
{
    class HttpContextCorrelationIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null) return;

            var correlationId = httpContext.Items[CorrelationIdRequestFilter.CorrelationIdHttpContextItemsKey];

            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("CorrelationId", correlationId ?? ""));
        }
    }
}