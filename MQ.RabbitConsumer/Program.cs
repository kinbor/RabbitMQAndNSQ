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
            Message_Pull();
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

                        /* 这里定义了一个消费者，用于消费服务器接受的消息
                         * C#开发需要注意下这里，在一些非面向对象和面向对象比较差的语言中，是非常重视这种设计模式的。
                         * 比如RabbitMQ使用了生产者与消费者模式，然后很多相关的使用文章都在拿这个生产者和消费者来表述。
                         * 但是，在C#里，生产者与消费者对我们而言，根本算不上一种设计模式，他就是一种最基础的代码编写规则。
                         * 所以，大家不要复杂的名词吓到，其实，并没那么复杂。
                         * 这里，其实就是定义一个EventingBasicConsumer类型的对象，然后该对象有个Received事件，该事件会在服务接收到数据时触发。
                         */
                        var consumer = new EventingBasicConsumer(channel);//消费者 
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

                        /* 这里定义了一个消费者，用于消费服务器接受的消息
                         * C#开发需要注意下这里，在一些非面向对象和面向对象比较差的语言中，是非常重视这种设计模式的。
                         * 比如RabbitMQ使用了生产者与消费者模式，然后很多相关的使用文章都在拿这个生产者和消费者来表述。
                         * 但是，在C#里，生产者与消费者对我们而言，根本算不上一种设计模式，他就是一种最基础的代码编写规则。
                         * 所以，大家不要复杂的名词吓到，其实，并没那么复杂。
                         * 这里，其实就是定义一个EventingBasicConsumer类型的对象，然后该对象有个Received事件，该事件会在服务接收到数据时触发。
                         */
                        var consumer = new EventingBasicConsumer(channel);//消费者 
                        channel.BasicConsume("jkQueue", true, consumer);//推模式：消费消息 autoAck参数为消费后是否删除
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine("Received： {0}", message);
                        };
                    }
                }

                var str = Console.ReadLine();
                while (str.ToUpper() != "STOP")
                {
                    str = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
