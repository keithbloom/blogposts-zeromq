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
            using (ZmqSocket ventilator = _context.CreateSocket(SocketType.PULL),
                sink = _context.CreateSocket(SocketType.PUSH),
                controller = _context.CreateSocket(SocketType.SUB))
            {
                ventilator.Connect("inproc://ventilator");
                sink.Connect("inproc://sink");

                SetupController(controller);

                _work = true;

                ventilator.ReceiveReady += (socket, events) =>
                    {
                        RecieverPollInHandler(ventilator, sink);
                    };

                var poller = new Poller(new[] {ventilator, controller});

                while (_work)
                {
                    poller.Poll();
                }

            }
        }


        private void RecieverPollInHandler(ZmqSocket reciever, ZmqSocket sender)
        {
            Thread.Sleep(100);
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

            Console.Write(".");

            sender.Send(fileLength.ToString(), Encoding.Unicode);
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