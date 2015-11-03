using Rebus.Config;
using Rebus.Persistence.SqlServer;
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
    }
}