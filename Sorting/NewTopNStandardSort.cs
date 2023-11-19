﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    public class NewTopNStandardSort<T> : ITopNSort<T>
    {
        public string Name { get { return "TopN ConcurrentBag"; } }

        public T[] TopNSort(T[] inputOutput, int n)
        {
            Array.Sort(inputOutput);
            return inputOutput.Take(n).ToArray();
        }

        public T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer)
        {
            Array.Sort(inputOutput, comparer);
            return inputOutput.Take(n).ToArray();
        }

    }
}
