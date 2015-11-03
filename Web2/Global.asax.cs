using System.Web;
using System.Web.Http;
using Shared;

namespace Web2
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Logging.Initialize("web2");

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
