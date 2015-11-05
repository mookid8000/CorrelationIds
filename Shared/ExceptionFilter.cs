using System.Web.Http.Filters;
using Serilog;

namespace Shared
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Log.Error(actionExecutedContext.Exception, "Oh no!");
        }
    }
}