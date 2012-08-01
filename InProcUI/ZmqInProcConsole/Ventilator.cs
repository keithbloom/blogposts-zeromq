using System;
using System.Text;
using ZMQ;

namespace ZmqInProcConsole
{
    public class Ventilator
    {
        private readonly Context _ventilator;
        private Socket _socket;

        public Ventilator(Context ventilator)
        {
            _ventilator = ventilator;
        }

        public void Start()
        {
            _socket = _ventilator.Socket(SocketType.PUSH);
            _socket.Bind("inproc://ventilator");
        }

        public void Run()
        {
            Console.WriteLine("Press enter when the workers are ready: ");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {

            }

            Console.WriteLine("Send tasks to workers..");

            _socket.Send("0", Encoding.Unicode);

            var randomthing = new Random(DateTime.Now.Millisecond);

            const int tasksToSend = 100;

            var expectedTime = 0;

            for (int i = 0; i < tasksToSend; i++)
            {
                var sleepTimeOnWorker = randomthing.Next(1, 100);
                expectedTime += sleepTimeOnWorker;
                _socket.Send(sleepTimeOnWorker.ToString(), Encoding.Unicode);
            }
            
        }

        public void Stop()
        {
            _socket.Dispose();
        }
    }
}
