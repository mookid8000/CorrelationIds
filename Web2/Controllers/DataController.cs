using System;
using System.Web.Http;

namespace Web2.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        public string GetNewData()
        {
            return $"the time is {DateTime.Now:G}";
        }
    }
}