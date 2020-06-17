using NsqSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.NsqProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var producer = new Producer("192.168.1.169:9530");
            producer.Publish("test-topic-name", "Hello!");

            Console.WriteLine("Enter your message (blank line to quit):");
            string line = Console.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                producer.Publish("test-topic-name", line);
                line = Console.ReadLine();
            }

            producer.Stop();
        }
    }
}
