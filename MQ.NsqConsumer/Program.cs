using NsqSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.NsqConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new Consumer for each topic/channel
            var consumer = new Consumer("test-topic-name", "channel-name");
            consumer.AddHandler(new MessageHandler());
            consumer.ConnectToNsqLookupd(new string[] { "192.168.1.169:9528"});

            Console.WriteLine("Listening for messages. If this is the first execution, it " +
                              "could take up to 60s for topic producers to be discovered.");
            Console.WriteLine("Press enter to stop...");
            Console.ReadLine();

            consumer.Stop();
        }
    }
    public class MessageHandler : IHandler
    {
        /// <summary>Handles a message.</summary>
        public void HandleMessage(IMessage message)
        {
            string msg = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine(msg);
        }

        /// <summary>
        /// Called when a message has exceeded the specified <see cref="Config.MaxAttempts"/>.
        /// </summary>
        /// <param name="message">The failed message.</param>
        public void LogFailedMessage(IMessage message)
        {
            // Log failed messages
        }
    }
}
