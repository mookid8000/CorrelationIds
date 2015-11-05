using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Rebus.Config;
using Rebus.Messages;
using Rebus.Persistence.SqlServer;
using Rebus.Pipeline;
using Rebus.Pipeline.Send;
using Rebus.Routing;
using Rebus.Routing.TypeBased;
using Rebus.Serilog;
using Rebus.Subscriptions;
using Serilog;
using Shared.Commands;

namespace Shared
{
    public static class Config
    {
        const string SqlServer = "server=.\\SQLEXPRESS; database=corr_ids; trusted_connection=true";

        public static void Routing(StandardConfigurer<IRouter> configurer)
        {
            configurer.TypeBased()
                .Map<DoStuffInTheBackground>("backend1");
        }

        public static void Subscriptions(StandardConfigurer<ISubscriptionStorage> configurer)
        {
            configurer.StoreInSqlServer(SqlServer, "rbs2_Subscriptions", isCentralized: true);
        }

        public static void Logging(RebusLoggingConfigurer configurer)
        {
            configurer.Serilog(Log.Logger);
        }

        public static void WebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new CorrelationIdRequestFilter());
            config.Filters.Add(new ExceptionFilter());
        }

        public static void TransferCorrelationIdFromHttpContextToOutgoingRebusMessages(this OptionsConfigurer configurer)
        {
            configurer.Decorate<IPipeline>(c =>
            {
                var pipeline = c.Get<IPipeline>();

                var step = new HttpContextCorrelationIdOutgoingMessagesStep();

                return new PipelineStepInjector(pipeline)
                    .OnSend(step, PipelineRelativePosition.Before, typeof (FlowCorrelationIdStep));
            });
        }

        class HttpContextCorrelationIdOutgoingMessagesStep : IOutgoingStep
        {
            public Task Process(OutgoingStepContext context, Func<Task> next)
            {
                var correlationId = HttpContext.Current?.Items[CorrelationIdRequestFilter.CorrelationIdHttpContextItemsKey]?.ToString();

                if (correlationId != null)
                {
                    var message = context.Load<Message>();

                    message.Headers[Headers.CorrelationId] = correlationId;
                }

                return next();
            }
        }
    }
}