using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace ZmqInProcConsole
{
    public class TaskWorker
    {
        private readonly Context context;

        public TaskWorker(Context context)
        {
            this.context = context;
        }

        public void Run()
        {
            using(Socket reciever = context.Socket(SocketType.PULL), sender = context.Socket(SocketType.PUSH))
            {
                Console.WriteLine("Worker started");

                reciever.Connect("inproc://ventilator");
                sender.Connect("inproc://sink");

                while (true)
                {
                    var task = reciever.Recv(Encoding.Unicode);

                    Console.WriteLine("{0}", task);

                    var sleepTime = Convert.ToInt32(task);
                    Thread.Sleep(sleepTime);

                    sender.Send("", Encoding.Unicode);
                }
            }
        }
    }
}