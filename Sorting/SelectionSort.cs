using System;
using System.Collections.Generic;

namespace Sorting
{
    public class SelectionSort<T> : ISort<T>
    {
        public string Name { get { return "SelectionSort"; } }

        public void Sort(T[] inputOutput)
        {
            Sort(inputOutput, Comparer<T>.Default);
        }

        public void Sort(T[] inputOutput, IComparer<T> comparer)
        {
            int n = inputOutput.Length;
            for (int i = 0; i < n - 1; i++) {
                int min = i;
                for (int j = i + 1; j < n; j++) {
                    if (comparer.Compare(inputOutput[j], inputOutput[min]) < 0) {
                        T tmp = inputOutput[j];
                        inputOutput[j] = inputOutput[min];
                        inputOutput[min] = tmp;
                    }
                }
            }
        }
    }
}
