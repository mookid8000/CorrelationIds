using System.Threading.Tasks;
using System.Web.Http;
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
            await WebApiApplication.Bus.Send(new DoStuffInTheBackground());

            return "done!";
        }
    }
}