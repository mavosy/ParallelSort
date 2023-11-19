using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    public class StandardSort<T> : ISort<T>
    {
        public string Name { get { return "StandardSort"; } }

        public void Sort(T[] inputOutput)
        {
            Array.Sort(inputOutput);
        }

        public void Sort(T[] inputOutput, IComparer<T> comparer)
        {
            Array.Sort(inputOutput, comparer);
        }
    }
}
