using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Shared
{
    class RequestIdFilter : ActionFilterAttribute
    {
        public const string CorrelationIdHttpContextItemsKey = "correlation-id";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (HttpContext.Current == null) return;

            var headers = actionContext.Request.Headers;

            IEnumerable<string> values;

            var correlationId = headers.TryGetValues("x-correlation-id", out values)
                ? values.First()
                : Guid.NewGuid().ToString("n");

            HttpContext.Current.Items[CorrelationIdHttpContextItemsKey] = correlationId;

            base.OnActionExecuting(actionContext);
        }
    }
}