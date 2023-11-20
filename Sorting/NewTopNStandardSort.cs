using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sorting
{
    public class NewTopNStandardSort<T> : ITopNSort<T> where T : IComparable<T>
    {
        public string Name { get { return "TopN ConcurrentBag"; } }

        public T[] TopNSort(T[] inputOutput, int n)
        {
            return TopNSort(inputOutput, n, Comparer<T>.Default);
        }

        public T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer)
        {
            //chunkSize är antalet element att sortera parallellt
            //Sidan 5, 47
            int threshold = (int)Math.Log(Environment.ProcessorCount, 2) + 1;
            int chunkSize = inputOutput.Length / Environment.ProcessorCount;

            //För små arrayer sorteras de sekventiellt (s. 46, 74)
            if (inputOutput.Length <= threshold || n <= threshold)
            {
                Array.Sort(inputOutput, comparer);
                return inputOutput.Take(n).ToArray();
            }

            ConcurrentBag<T> topNBag = new ConcurrentBag<T>();

            //Sortera varje chunkSize parallellt (s. 70)
            //Använd PLINQ istället
            Parallel.ForEach(Partitioner.Create(0, inputOutput.Length, chunkSize), range =>
            {
                //kopierar arrayen för att undvika "Anti-pattern" (s. 24)
                //detta skulle kunna ha lösts med t.ex. locks
                T[] chunk = new T[range.Item2 - range.Item1];
                Array.Copy(inputOutput, range.Item1, chunk, 0, chunk.Length);

                Array.Sort(chunk, comparer);

                //Lägg till element 0 -> n i ConcurrentBag
                for (int i = 0; i < n && i < chunk.Length; i++)
                {
                    topNBag.Add(chunk[i]);
                }
            });

            //Slå ihop alla element från ConcurrentBags till en array och sortera den
            T[] consolidatedTopN = topNBag.ToArray();
            Array.Sort(consolidatedTopN, comparer);

            //skapar en slutgiltig array med storlek n och kopierar över elementen
            T[] finalResult = new T[n];
            Array.Copy(consolidatedTopN, finalResult, n);

            return finalResult;
        }
    }
}
