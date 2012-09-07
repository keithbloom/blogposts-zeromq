using System;
using System.Text;
using ZeroMQ;

namespace AltNetPresentation
{
    public class Sink
    {
        private readonly ZmqContext _context;
        private ZmqSocket _receiver;
        private ZmqSocket _controller;

        public Sink(ZmqContext context)
        {
            _context = context;
        }

        public void Start()
        {
            _receiver = _context.CreateSocket(SocketType.PULL);
            _receiver.Bind("inproc://sink");

            ConfigureController();
        }



        public Int64 Run(int length)
        {
            InitSink();

            Int64 sizeOfDirectory = 0;
            
            for (var i = 0; i < length; i++)
            {
                var size = _receiver.Receive(Encoding.Unicode);
                Int64 temp;
                if(Int64.TryParse(size, out temp))
                {
                    sizeOfDirectory += temp;
                }
            }
            
            EndSink();

            return sizeOfDirectory;
        }

        private void EndSink()
        {
            _controller.Send("KILL", Encoding.Unicode);

            Console.WriteLine();
            Console.WriteLine("[SINK] Finsihed");
        }

        private void InitSink()
        {
            Console.WriteLine("[SINK] Start listening for results");
            _receiver.Receive(Encoding.Unicode);
        }

        public void Stop()
        {
            _receiver.Dispose();
            _controller.Dispose();
        }

        private void ConfigureController()
        {
            _controller = _context.CreateSocket(SocketType.PUB);
            _controller.Bind("inproc://controller");
        }

    }
}