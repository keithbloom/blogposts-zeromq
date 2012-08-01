using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace ZmqInProcConsole
{
    public class TaskWorker
    {
        private readonly Context context;
        private bool _work;

        public TaskWorker(Context context)
        {
            this.context = context;
        }

        public void Run()
        {
            using(Socket reciever = context.Socket(SocketType.PULL), 
                sender = context.Socket(SocketType.PUSH),
                controller = context.Socket(SocketType.SUB))
            {
                Console.WriteLine("Worker started");

                reciever.Connect("inproc://ventilator");
                sender.Connect("inproc://sink");
                
                
                controller.Subscribe("",Encoding.Unicode);
                controller.Connect("inproc://controller");
                

                _work = true;

                var items = new PollItem[2];
                items[0] = reciever.CreatePollItem(IOMultiPlex.POLLIN);
                items[0].PollInHandler += (socket, revents) => RecieverPollInHandler(reciever, sender);

                items[1] = controller.CreatePollItem(IOMultiPlex.POLLIN);
                items[1].PollInHandler += (x, y) =>
                    {
                        Console.WriteLine("I'm done for the day");
                        _work = false;
                    };
 
                while (_work)
                {
                    context.Poll(items);
                }

            }
        }

        private void RecieverPollInHandler(Socket reciever, Socket sender)
        {
            var task = reciever.Recv(Encoding.Unicode);

            Console.WriteLine("{0}", task);

            var sleepTime = Convert.ToInt32(task);
            Thread.Sleep(sleepTime);

            sender.Send("", Encoding.Unicode);
        }

        public void Stop()
        {
            _work = false;
        }
    }
}