using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace ZmqInProcConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Context(1))
            {
                using(var socket = context.Socket(SocketType.PAIR))
                {
                    socket.Bind("inproc://step3");

                    var step2 = new Thread(Step2);
                    step2.Start(context);

                    Console.WriteLine("Waiting for the result");

                    var result = socket.Recv(Encoding.Unicode);

                    Console.WriteLine("Test Successfull!! {0}",result);
                }
            }
        }

        static void Step2(object context)
        {
            string message;

            using(var reciever = ((Context)context).Socket(SocketType.PAIR))
            {
                reciever.Bind("inproc://step2");

                var step1 = new Thread(Step1);
                step1.Start(context);

                message = reciever.Recv(Encoding.Unicode);
            }

            using(var sender = ((Context)context).Socket(SocketType.PAIR))
            {

                Thread.Sleep(1000);

                Console.WriteLine("Step2 sending message");
                sender.Connect("inproc://step3");
                sender.Send("Hello " + message,Encoding.Unicode);
            }
        }

        static void Step1(object context)
        {
            using (var sender = ((Context)context).Socket(SocketType.PAIR))
            {
                Thread.Sleep(1000);
                Console.WriteLine("Step1 sending message");
                sender.Connect("inproc://step2");
                sender.Send("Matey", Encoding.Unicode);
            }
        }
    }
}
