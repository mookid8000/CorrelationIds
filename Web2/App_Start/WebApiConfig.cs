using System.Web.Http;
using Shared;

namespace Web2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Config.WebApi(config);
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
