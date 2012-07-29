using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace PubServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {
                using (var publish = context.Socket(SocketType.PUB))
                {
                    publish.Bind("tcp://*:5566");

                    var count = 0;

                    while (true)
                    {
                        Console.WriteLine(string.Format("About to send {0}",count));
                        publish.Send(count++.ToString(), Encoding.Unicode);
                        Thread.Sleep(5000);
                    }
                }
            }

        }
    }
}
