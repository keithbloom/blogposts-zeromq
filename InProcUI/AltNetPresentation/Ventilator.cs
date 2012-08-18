using System;
using System.Text;
using ZeroMQ;

namespace AltNetPresentation
{
    public class Ventilator
    {
        private readonly ZmqContext _context;
        private ZmqSocket _ventilator;

        public Ventilator(ZmqContext ventilator)
        {
            _context = ventilator;
        }

        public void Start()
        {
            _ventilator = _context.CreateSocket(SocketType.PUSH);
            _ventilator.Bind("inproc://ventilator");
        }

        public void Run(string[] fileList)
        {
            Console.WriteLine("[VENTILATOR] Send tasks to workers..");
            _ventilator.Send("0", Encoding.Unicode);

            Console.WriteLine("[VENTILATOR] Finding the size of {0} files", 
                fileList.Length);

            foreach (var fileName in fileList)
            {
                _ventilator.Send(fileName, Encoding.Unicode);
            }
        }

        public void Stop()
        {
            _ventilator.Dispose();
        }
    }
}