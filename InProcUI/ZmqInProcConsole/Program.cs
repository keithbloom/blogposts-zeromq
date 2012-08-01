using System;
using System.Threading;
using ZMQ;

namespace ZmqInProcConsole
{
    class Program
    {
        static void Main(string[] args)
        {


            using (var context = new Context(1))
            {
                var ventilator = new Ventilator(context);
                var sink = new Sink(context);

                ventilator.Start();
                sink.Start();

                Console.WriteLine("Hit enter to start");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                const int workersCount = 10;
                var workers = new Thread[workersCount];

                for (int i = 0; i < workersCount; i++)
                {
                    (workers[i] = new Thread(() => new TaskWorker(context).Run())).Start();
                }
                
                ventilator.Run();
                sink.Run();

                Console.WriteLine("All done");

                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                foreach (var thread in workers)
                {
                    thread.Abort();
                }

                ventilator.Stop();
                sink.Stop();

             
            }

        }

        

        
    }
}
