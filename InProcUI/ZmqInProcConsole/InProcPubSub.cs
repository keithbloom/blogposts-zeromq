using System;
using System.Text;
using System.Threading;
using ZMQ;

namespace ZmqInProcConsole
{
    public class InProcPubSub
    {
        public void Run()
        {
            using (var context = new Context(1))
            {
                using (var pub = context.Socket(SocketType.PUB))
                using (var control = context.Socket(SocketType.SUB))
                {
                    pub.Bind("inproc://workers");
                    
                    var step2 = new Thread(() => Worker(context));
                    step2.Start();
                    Thread.Sleep(100);

                    control.Connect("inproc://control");
                    control.Subscribe("", Encoding.Unicode);

                    var count = 0;
                    var message = string.Empty;
                    var controlItems = new PollItem[2];

                    controlItems[0] = control.CreatePollItem(IOMultiPlex.POLLIN);
                    controlItems[0].PollInHandler += (x, y) =>
                        {
                            message = x.Recv(Encoding.Unicode);
                            Console.WriteLine("A message: {0}", message);
                        };

                    controlItems[1] = pub.CreatePollItem(IOMultiPlex.POLLOUT);
                    controlItems[1].PollOutHandler += (z, y) =>
                        {
                            Console.WriteLine(string.Format("About to send {0}", count));
                            z.Send(count++.ToString(), Encoding.Unicode);
                        };

                    //Console.WriteLine("Recieved the {0} signal",control.Recv(Encoding.Unicode));

                    while (string.IsNullOrEmpty(message) || !message.Equals("Stop"))
                    {
                        context.Poll(controlItems, -1);
                        Thread.Sleep(500);
                    }
                }
            }
        }

        public void Worker(object things)
        {
            {
                var context = (Context)things;
                using (var control = context.Socket(SocketType.PUB))
                using (var sub = context.Socket(SocketType.SUB))
                {
                    Console.WriteLine("Worker started");
                    control.Bind("inproc://control");

                    sub.Subscribe("", Encoding.Unicode);
                    sub.Connect("inproc://workers");

                    var count = 0;

                    var items = new PollItem[1];

                    items[0] = sub.CreatePollItem(IOMultiPlex.POLLIN);
                    items[0].PollInHandler += (x, y) =>
                        {
                            Console.WriteLine("Received : {0}",x.Recv(Encoding.Unicode));
                            count++;
                        };

                    control.Send("Go", Encoding.Unicode);

                    while (count < 10)
                    {
                        context.Poll(items, -1);
                    }

                    control.Send("Stop", Encoding.Unicode);
                    
                }
            }
        }
    }
}