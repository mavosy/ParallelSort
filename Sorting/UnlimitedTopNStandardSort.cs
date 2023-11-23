using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sorting
{
    public class UnlimitedTopNStandardSort<T> : ITopNSort<T> where T : IComparable<T>
    {
        public string Name { get { return "Unlimited TopN Sort"; } }

        public T[] TopNSort(T[] inputOutput, int n)
        {
            return TopNSort(inputOutput, n, Comparer<T>.Default);
        }

        public T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer)
        {
            //För små arrayer sorteras de sekventiellt (godtyckligt satt till 2*antal trådar)
            if (inputOutput.Length <= Process.GetCurrentProcess().Threads.Count*2)
            {
                Array.Sort(inputOutput, comparer);
                return inputOutput.Take(n).ToArray();
            }

            //Metoden ParallelTopNSort tar en extra parameter som anger antalet trådar metoden får använda. Där -1 är max antal möjliga kärnor eller 4 kärnor.
            //Vi skulle också kunna sätta degree till Environment.ProcessorCount för att använda alla tillgängliga kärnor
            return ParallelTopNSort(inputOutput, n, comparer, -1);
        }

        private T[] ParallelTopNSort(T[] inputOutput, int n, IComparer<T> comparer, int degree)
        {
            //Försökte se om jag kunde optimera hastigheten genom att dela upp partionerna
            //propertionerligt mot antal lediga trådar. Crash genom AggregateException
            ////ThreadPool.GetAvailableThreads(out int threads, out int notApplicableThreads);

            //Anger hur många kärnor som är lediga
            int threads = Environment.ProcessorCount;
            
            //Vi använder partitioner för att dela upp arrayen i mindre delar baserat på trådar
            //https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.partitioner?view=net-7.0
            var partitions = Partitioner.Create(0, inputOutput.Length, inputOutput.Length / threads);
            
            //Vi använder collections Concurrent eftersom de är thread-safe
            ConcurrentBag<T> bag = new ConcurrentBag<T>();

            //Parallel.ForEach tar en partitioner, en lambda-funktion och en ParallelOptions
            Parallel.ForEach(partitions, new ParallelOptions
            {
                MaxDegreeOfParallelism = degree
            }, i =>
            {
                //Vi sorterar varje partition
                Array.Sort(inputOutput, i.Item1, i.Item2 - i.Item1, comparer);

                //för varje partition lägger vi till de n första elementen i ConcurrentBag
                foreach (var item in inputOutput.Skip(i.Item1).Take(n))
                {
                    bag.Add(item);
                }

            });

            //Vi sorterar med PLINQ och tar de n första elementen
            //PLNIQ används för att fortsätta använda parallellism
            var sortedArray = bag.ToArray().AsParallel().OrderBy(x => x, comparer).Take(n).ToArray();

            return sortedArray;
        }
    }
}
