using System;
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
                    Log.Information("Done doing stuff - notifying subscribers that I did it now....");

                    await bus.Publish(new DidStuffInTheBackground());
                });

                Configure.With(activator)
                    .Logging(Config.Logging)
                    .Transport(t => t.UseMsmq("backend1"))
                    .Subscriptions(Config.Subscriptions)
                    .Routing(Config.Routing)
                    .Start();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
