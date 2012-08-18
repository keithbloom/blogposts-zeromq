using System;
using System.IO;
using System.Text;
using System.Threading;
using ZeroMQ;

namespace AltNetPresentation
{
    public class TaskWorker
    {
        private readonly ZmqContext _context;
        private bool _work;

        public TaskWorker(ZmqContext context)
        {
            _context = context;
        }

        public void Run()
        {
            using (ZmqSocket reciever = _context.CreateSocket(SocketType.PULL),
                sender = _context.CreateSocket(SocketType.PUSH),
                controller = _context.CreateSocket(SocketType.SUB))
            {
                Console.WriteLine("Worker started");

                reciever.Connect("inproc://ventilator");
                sender.Connect("inproc://sink");

                SetupController(controller);

                _work = true;

                reciever.ReceiveReady += (socket, events) =>
                    {
                        RecieverPollInHandler(reciever, sender);
                    };

                var poller = new Poller(new[] {reciever, controller});

                while (_work)
                {
                    poller.Poll();
                }

            }
        }


        /* This is the place to read the file size */
        private void RecieverPollInHandler(ZmqSocket reciever, ZmqSocket sender)
        {
            var fileToMeasure = reciever.Receive(Encoding.Unicode);
            
            Int64 fileLength = 0;
            FileStream fs = null;

            try
            {
                fs = File.OpenRead(fileToMeasure);
                fileLength = fs.Length;
            }
            catch (IOException)
            {
            }
            finally
            {
                if (fs != null) fs.Dispose();
            }

            Console.WriteLine("The length is: {0}",fileLength);

            sender.Send(fileLength.ToString(), Encoding.Unicode);
            //Thread.Sleep(10);
        }


        private void SetupController(ZmqSocket controller)
        {
            controller.SubscribeAll();
            controller.Connect("inproc://controller");
            controller.ReceiveReady += (x, y) =>
            {
                //Console.WriteLine("I'm done for the day");
                _work = false;
            };
        }

        public void Stop()
        {
            _work = false;
        }
    }
}