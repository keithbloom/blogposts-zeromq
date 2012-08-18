using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ZmqInProcConsole
{
    public class DirectoryCounterSharedState
    {
        private static Int64 DirectoryBytes(string path, string searchPattern, SearchOption searchOption)
        {
            var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
            Int64 masterTotal = 0;

            ParallelLoopResult result = Parallel.ForEach<string, Int64>(
                files,

                () => 0,

                (file, loopState, index, taskLocalTotal) =>
                    {
                        Int64 fileLength = 0;

                        FileStream fs = null;

                        try
                        {
                            fs = File.OpenRead(file);
                            fileLength = fs.Length;
                        }
                        catch (IOException)
                        {
                        }
                        finally
                        {
                            if (fs != null) fs.Dispose();
                        }

                        return taskLocalTotal + fileLength;
                    },

                taskLocalTotal => Interlocked.Add(ref masterTotal, taskLocalTotal));

            return masterTotal;
        }
    }
}