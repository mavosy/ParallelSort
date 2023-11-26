using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    public class PLINQTopNSort<T> : ITopNSort<T> where T : IComparable<T>
    {
        public string Name { get { return "PLINQ TopN Sort"; } }

        public T[] TopNSort(T[] inputOutput, int n)
        {
            return TopNSort(inputOutput, n, Comparer<T>.Default);
        }

        public T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer)
        {
            var sortedArray = inputOutput.AsParallel().AsOrdered().OrderBy(x => x, comparer).Take(n).ToArray();

            return sortedArray;
        }
    }
}
