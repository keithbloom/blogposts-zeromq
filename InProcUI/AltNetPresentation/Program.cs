using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using ZeroMQ;

namespace AltNetPresentation
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = ZmqContext.Create())
            {
                var ventilator = new Ventilator(context);
                var sink = new Sink(context);
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                ventilator.Start();
                sink.Start();

                Console.WriteLine("Hit enter to start");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                const int workersCount = 5;
                var workers = new Thread[workersCount];

                for (int i = 0; i < workersCount; i++)
                {
                    (workers[i] = new Thread(() => new TaskWorker(context).Run())).Start();
                }

                var fileList = EnumerateDirectory(@"C:\Users\keith\Downloads", "*.*", SearchOption.AllDirectories);

                ventilator.Run(fileList);
                var result = sink.Run(fileList.Length);

                Console.WriteLine();
                Console.WriteLine("Found the length of {0} files in {1} milliseconds.\nDirectory size is {2}",fileList.Length, stopWatch.ElapsedMilliseconds, result);

                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                foreach (var thread in workers)
                {
                    thread.Abort();
                }

                ventilator.Stop();
                sink.Stop();
            }


        }

        public static string[] EnumerateDirectory(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(path, searchPattern, searchOption).ToArray();
        }
    }
}
