using System;
using System.Text;
using ZMQ;

namespace SubClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var context = new Context(1))
            {
                using (var subscriber = context.Socket(SocketType.SUB))
                {
                    subscriber.Subscribe("",Encoding.Unicode);
                    subscriber.Connect("tcp://localhost:5566");

                    while(true)
                    {
                        var message = subscriber.Recv(Encoding.Unicode);
                        Console.WriteLine(string.Format("Revieved {0}",message));
                    }
                }
            }
        }
    }
}
