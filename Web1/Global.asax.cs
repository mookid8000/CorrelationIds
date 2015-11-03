using System.Web;
using System.Web.Http;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Serilog;
using Rebus.Transport.Msmq;
using Serilog;
using Shared;

namespace Web1
{
    public class WebApiApplication : HttpApplication
    {
        public static IBus Bus;

        protected void Application_Start()
        {
            Logging.Initialize("web1");

            Bus = Configure.With(new BuiltinHandlerActivator())
                .Logging(Config.Logging)
                .Transport(t => t.UseMsmqAsOneWayClient())
                .Routing(Config.Routing)
                .Start();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_End()
        {
            Bus?.Dispose();
        }
    }
}
