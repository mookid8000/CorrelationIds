using System;
using System.Net.Http;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Rebus.CastleWindsor;
using Rebus.Config;
using Rebus.Messages;
using Rebus.Pipeline;
using Rebus.Transport.Msmq;
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

            using (var container = new WindsorContainer())
            {
                container.RegisterHandler<DidStuffInTheBackgroundHandler>();

                container.Register(
                    Component.For<HttpClient>()
                        .UsingFactoryMethod(k =>
                        {
                            var messageContext = k.Resolve<IMessageContext>();

                            var client = new HttpClient();

                            var correlationId = messageContext.Message.Headers[Headers.CorrelationId];
                            client.DefaultRequestHeaders.Add("x-correlation-id", correlationId);

                            return client;
                        })
                        .LifestylePerRebusMessage()
                    );

                var bus = Configure.With(new CastleWindsorContainerAdapter(container))
                    .Logging(Config.Logging)
                    .Transport(t => t.UseMsmq("backend2"))
                    .Subscriptions(Config.Subscriptions)
                    .Routing(Config.Routing)
                    .Options(o => o.SetMaxParallelism(50))
                    .Start();

                bus.Subscribe<DidStuffInTheBackground>().Wait();

                Console.WriteLine("Press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
