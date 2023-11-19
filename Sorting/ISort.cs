using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utilities;

namespace Sorting
{
    public interface ISort<T> : ICompute
    {
        void Sort(T[] inputOutput);
        void Sort(T[] inputOutput, IComparer<T> comparer);
    }
}
