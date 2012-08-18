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
            _receiver.Receive(Encoding.Unicode);
            Int64 sizeOfDirectory = 0;

            for (int i = 0; i < length; i++)
            {
                var size = _receiver.Receive(Encoding.Unicode);
                Int64 temp;
                if(Int64.TryParse(size, out temp))
                {
                    sizeOfDirectory += temp;
                }
            }
            
            _controller.Send("KILL", Encoding.Unicode);

            return sizeOfDirectory;
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