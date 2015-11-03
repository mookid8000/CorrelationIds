using System.Web;
using System.Web.Http;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Transport.Msmq;
using Shared;

namespace Web1
{
    public class WebApiApplication : HttpApplication
    {
        public static IBus Bus;

        protected void Application_Start()
        {
            Logging.Initialize("web2");

            Bus = Configure.With(new BuiltinHandlerActivator())
                .Logging(Config.Logging)
                .Transport(t => t.UseMsmqAsOneWayClient())
                .Routing(Config.Routing)
                .Options(o => o.TransferCorrelationIdFromHttpContextToOutgoingRebusMessages())
                .Start();

            GlobalConfiguration.Configure(Config.WebApi);
        }

        protected void Application_End()
        {
            Bus?.Dispose();
        }
    }
}
