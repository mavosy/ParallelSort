using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utilities;

namespace Sorting
{
    public interface ITopNSort<T> : ICompute
    {
        T[] TopNSort(T[] inputOutput, int n);
        T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer);
    }
}
