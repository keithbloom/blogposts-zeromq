using System;
using System.Diagnostics;
using System.Text;
using ZMQ;

namespace ZmqInProcConsole
{
    public class Sink
    {
        private Context _context;
        private Socket _receiver;

        public Sink(Context context)
        {
            this._context = context;
        }

        public void Start()
        {
            _receiver = _context.Socket(SocketType.PULL);
            _receiver.Bind("inproc://sink");
        }

        public void Run()
        {
            _receiver.Recv();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            const int tasksToConfirm = 100;

            for (int i = 0; i < tasksToConfirm; i++)
            {
                var message = _receiver.Recv(Encoding.Unicode);
                Console.Write(i % 10 == 0 ? ":" : ".");
            }
            stopwatch.Stop();
                
            Console.WriteLine("Total elapsed time: {0}",stopwatch.ElapsedMilliseconds);
            
        }

        public void Stop()
        {
            _receiver.Dispose();
        }
    }
}