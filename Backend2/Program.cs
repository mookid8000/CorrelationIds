using System;
using System.Net.Http;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Messages;
using Rebus.Transport.Msmq;
using Serilog;
using Shared;
using Shared.Events;
#pragma warning disable 1998

namespace Backend2
{
    class Program
    {
        static void Main()
        {
            Logging.Initialize("backend2");

            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Handle<DidStuffInTheBackground>(async (_, context, message) =>
                {
                    Log.Information("I just learned that someone did stuff in the background - I'll get some data");

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("x-correlation-id", context.Message.Headers[Headers.CorrelationId]);

                        var data = await client.GetStringAsync("http://localhost:64599/api/data");

                        Log.Information("I'm done doing stuff now.... here's the data: {Data}", data);
                    }
                });

                var bus = Configure.With(activator)
                    .Logging(Config.Logging)
                    .Transport(t => t.UseMsmq("backend2"))
                    .Subscriptions(Config.Subscriptions)
                    .Routing(Config.Routing)
                    .Start();

                bus.Subscribe<DidStuffInTheBackground>().Wait();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
