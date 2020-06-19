using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQ.RabbitConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Message_Pull();
            Message_Push();
        }

        static void Message_Pull()
        {
            try
            {
                var factory = new ConnectionFactory();
                factory.HostName = "192.168.1.169";
                factory.Port = 5672;
                factory.UserName = "jbkb";
                factory.Password = "jbkb@12345";

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare("jkQueue", false, false, false, null);

                        BasicGetResult result = null;
                        do
                        {
                            result = channel.BasicGet("jkQueue", true);//拉模式：消费消息 autoAck参数为消费后是否删除
                            if (result != null)
                            {
                                var body = result.Body.ToArray();
                                var message = Encoding.UTF8.GetString(body);
                                Console.WriteLine("Received： {0}", message);
                            }
                        }
                        while (true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void Message_Push()
        {
            try
            {
                var factory = new ConnectionFactory();
                factory.HostName = "192.168.1.169";
                factory.Port = 5672;
                factory.UserName = "jbkb";
                factory.Password = "jbkb@12345";

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare("jkQueue", false, false, false, null);

                        var consumer = new EventingBasicConsumer(channel);//消费者 
                        channel.BasicConsume("jkQueue", true, consumer);//推模式：消费消息 autoAck参数为消费后是否删除
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine("Received： {0}", message);
                        };

                        var str = Console.ReadLine();
                        while (str.ToUpper() != "STOP")
                        {
                            str = Console.ReadLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
