using System;
using HyperIoC;
using HyperMsg.Broker;
using HyperMsg.Broker.Data;

namespace HyperMsg.BrokerHost
{
    class Program
    {
        private static Factory _factory;

        static void Main(string[] args)
        {
            Console.WriteLine("HyperMsg Broker Host");
            Console.WriteLine();
            bool initialised;

            try
            {
                Console.WriteLine("Initialising...");
                _factory = FactoryBuilder.Build().WithProfile<AnyProfile>().Create();
                
                initialised = _factory != null;
                Console.WriteLine("Initialisation complete.");
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                initialised = false;
            }

            if (initialised)
            {
                Console.WriteLine("Initialising the broker...");
                IDatabaseFactory databaseFactory = null;
                IBrokerManager broker = null;

                try
                {
                    databaseFactory = _factory.Get<IDatabaseFactory>();
                    databaseFactory.Create();

                    broker = _factory.Get<IBrokerManager>();
                    broker.Run();

                    Console.WriteLine("Broker is ready for messages...press return to exit.");
                    Console.ReadLine();
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }
                finally
                {
                    databaseFactory?.Dispose();
                    broker?.Dispose();
                }
            }
        }
    }
}
