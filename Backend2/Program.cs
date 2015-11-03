using System;
using Rebus.Activation;
using Rebus.Config;
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
                activator.Handle<DidStuffInTheBackground>(async message =>
                {
                    Log.Information("I'm done doing stuff now....");
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
