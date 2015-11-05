using System;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Transport.Msmq;
using Serilog;
using Shared;
using Shared.Commands;
using Shared.Events;

namespace Backend1
{
    class Program
    {
        static void Main()
        {
            Logging.Initialize("backend1");

            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Handle<DoStuffInTheBackground>(async (bus, message) =>
                {
                    Log.Information("I'm the backend and I'm doing stuff now");

                    await Task.Delay(TimeSpan.FromSeconds(1));

                    Log.Information("Now I'm done doing stuff - notifying subscribers!");

                    await bus.Publish(new DidStuffInTheBackground());
                });

                Configure.With(activator)
                    .Logging(Config.Logging)
                    .Transport(t => t.UseMsmq("backend1"))
                    .Subscriptions(Config.Subscriptions)
                    .Routing(Config.Routing)
                    .Options(o => o.SetMaxParallelism(50))
                    .Start();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
