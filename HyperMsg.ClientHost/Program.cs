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
                using (var provider = new RemoteMessageProvider(new ConfigSettings()))
                {
                    Console.ReadLine();

                    Console.WriteLine("Sending 100 messages to test end point...");

                    for (var i = 0; i < 100; i++)
                    {
                        var message = new BrokeredMessage { EndPoint = "test" };
                        message.SetBody(new User { Forename = "Homer", Surname = "Simpson" });
                        provider.Send(message);
                    }

                    Console.WriteLine("Sending 100 messages to test2 end point...");

                    for (var i = 0; i < 100; i++)
                    {
                        var message = new BrokeredMessage { EndPoint = "test2" };
                        message.SetBody(new User { Forename = "Marge", Surname = "Simpson" });
                        provider.Send(message);
                    }

                    Console.WriteLine("Retrieving 100 messages from test2 end point...");

                    var messages = provider.ReceiveAndDelete<BrokeredMessage>("test2", 10).ToList();
                    var counter = 0;

                    while (messages.Count > 0)
                    {
                        foreach (var message in messages)
                        {
                            var user = message.GetBody<User>();
                            Console.WriteLine("({0}) {1} {2}", ++counter, user.Forename, user.Surname);
                        }

                        messages = provider.ReceiveAndDelete<BrokeredMessage>("test2", 10).ToList();
                    }

                    Console.WriteLine("Checking that all receive and delete messages are removed...");
                    messages = provider.ReceiveAndDelete<BrokeredMessage>("test2", 10).ToList();

                    Console.WriteLine("Count is {0}", messages.Count);

                    Console.WriteLine("Retrieving 100 messages from test end point...");

                    messages = provider.Receive<BrokeredMessage>("test", 100).ToList();
                    counter = 0;

                    foreach (var message in messages)
                    {
                        var user = message.GetBody<User>();
                        Console.WriteLine("({0}) {1} {2}", ++counter, user.Forename, user.Surname);
                    }

                    Console.WriteLine("Checking that all receive messages still exist...");
                    messages = provider.Receive<BrokeredMessage>("test", 100).ToList();

                    Console.WriteLine("Count is {0}", messages.Count);

                    provider.Complete(messages.Take(50).ToArray());
                    provider.Abandon(messages.Skip(50).Take(50).ToArray());

                    Console.WriteLine("Checking that all abandoned messages still exist...");
                    messages = provider.Receive<BrokeredMessage>("test", 100).ToList();

                    Console.WriteLine("Count is {0}", messages.Count);

                    provider.Complete(messages.ToArray());

                    messages = provider.Receive<BrokeredMessage>("test", 100).ToList();

                    Console.WriteLine("Completed previously abandoned messages: Count is {0}", messages.Count);

                    provider.Dispose();

                    //var message = new BrokeredMessage {EndPoint = "test"};
                    //message.SetBody(new User {Forename = "Homer", Surname = "Simpson"});

                    //provider.Send(message);

                    //message = new BrokeredMessage {EndPoint = "test2"};
                    //message.SetBody(new User {Forename = "Marge", Surname = "Simpson"});
                    //provider.Send(message);

                    //message = new BrokeredMessage {EndPoint = "test"};
                    //message.SetBody(new User {Forename = "Bart", Surname = "Simpson"});
                    //provider.Send(message);

                    //message = new BrokeredMessage {EndPoint = "test2"};
                    //message.SetBody(new User {Forename = "Lisa", Surname = "Simpson"});
                    //provider.Send(message);

                    //Console.WriteLine("Added messages. Press return to receive");
                    //Console.ReadLine();

                    //var messages = provider.ReceiveAndDelete<BrokeredMessage>("test2", 10);

                    //foreach (var msg in messages)
                    //{
                    //    var user = msg.GetBody<User>();
                    //    Console.WriteLine("{0} {1}", user.Forename, user.Surname);
                    //}

                    //Console.WriteLine("Recieve messages. Press return to check acknowledgement");
                    //Console.ReadLine();

                    //messages = provider.ReceiveAndDelete<BrokeredMessage>("test2", 10);
                    //Console.WriteLine("Found {0} messages", messages.Count());
                }
            }
            catch (Exception error)
            {
                Console.Write(error);
            }

            Console.ReadLine();
        }
    }

    public class User
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
    }
}
