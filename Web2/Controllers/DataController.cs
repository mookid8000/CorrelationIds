using System;
using System.Web.Http;
using Serilog;

namespace Web2.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        [Route("")]
        [HttpGet]
        public string GetNewData()
        {
            Log.Information("Someone is asking about some data - I'll return some kind of timestamp");

            var now = DateTime.Now;

            if ((int) now.TimeOfDay.TotalSeconds%2 == 0)
            {
                throw new ApplicationException("wootadafook?!1 something happened!!!!1");
            }

            return $"the time is {now:G}";
        }
    }
}