using System;
using System.Linq;
using System.Runtime.Serialization;
using HyperMsg.Config;
using HyperMsg.Messages;
using HyperMsg.Providers;

namespace HyperMsg.ClientHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running the test client...Press return to start");

            try
            {
                Console.ReadLine();
                var message = new BrokeredMessage { EndPoint = "test", Persistent = true };
                message.SetBody(new User { Forename = "Homer", Surname = "Simpson" });
                var provider = new RemoteMessageProvider(new ConfigSettings());
                provider.Send(message);

                message = new BrokeredMessage { EndPoint = "test2" };
                message.SetBody(new User { Forename = "Marge", Surname = "Simpson" });
                provider.Send(message);

                message = new BrokeredMessage { EndPoint = "test" };
                message.SetBody(new User { Forename = "Bart", Surname = "Simpson" });
                provider.Send(message);

                message = new BrokeredMessage { EndPoint = "test2" };
                message.SetBody(new User { Forename = "Lisa", Surname = "Simpson" });
                provider.Send(message);

                Console.WriteLine("Added messages. Press return to receive");
                Console.ReadLine();

                var messages = provider.Receive<BrokeredMessage>("test2", 10);

                foreach (var msg in messages)
                {
                    var user = msg.GetBody<User>();
                    Console.WriteLine("{0} {1}", user.Forename, user.Surname);
                }

                Console.WriteLine("Recieve messages. Press return to check acknowledgement");
                Console.ReadLine();

                messages = provider.Receive<BrokeredMessage>("test2", 10);
                Console.WriteLine("Found {0} messages", messages.Count());
            }
            catch (Exception error)
            {
                Console.Write(error);
            }

            Console.ReadLine();
        }
    }

    [DataContract]
    public class User
    {
        [DataMember]
        public string Forename { get; set; }
        [DataMember]
        public string Surname { get; set; }
    }
}
