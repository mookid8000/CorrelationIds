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

            return $"the time is {DateTime.Now:G}";
        }
    }
}