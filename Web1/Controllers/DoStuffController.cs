using System.Threading.Tasks;
using System.Web.Http;
using Serilog;
using Shared.Commands;

namespace Web1.Controllers
{
    [RoutePrefix("api/dostuff")]
    public class DoStuffController : ApiController
    {
        [Route("justdoit")]
        [HttpPost]
        public async Task<string> JustDoIt()
        {
            Log.Information("I'll tell my backend to do stuff now");

            await WebApiApplication.Bus.Send(new DoStuffInTheBackground());

            return "done!";
        }
    }
}