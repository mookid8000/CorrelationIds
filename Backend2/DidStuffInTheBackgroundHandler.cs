using System.Net.Http;
using System.Threading.Tasks;
using Rebus.Handlers;
using Serilog;
using Shared.Events;

namespace Backend2
{
    class DidStuffInTheBackgroundHandler : IHandleMessages<DidStuffInTheBackground>
    {
        readonly HttpClient _httpClient;

        public DidStuffInTheBackgroundHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Handle(DidStuffInTheBackground message)
        {
            Log.Information("I just learned that someone did stuff in the background - I'll get some data");

            var data = await _httpClient.GetStringAsync("http://localhost:64599/api/data");

            Log.Information("I'm done doing stuff now.... here's the data: {Data}", data);
        }
    }
}