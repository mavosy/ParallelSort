using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sorting
{
    //klassen kräver att typen T ska vara jämförbar med sig själv
    public class ParallelMergeSort<T> : ISort<T> where T : IComparable<T>
    {
        // Sätter ett gränsvärde för storleken på arrayen. Vid små arrayer ska InsertionSort användas istället för MergeSort, för att undvika deoptimisering.
        // Värdet kan behöva optimeras efter testning.
        // Alternativt låter man storleken på arrayen inputOutput bestämma threshold.

        //private static readonly int _arraySizeThresholdFactor = 2;
        private readonly int _threshold = Environment.ProcessorCount * 2;

        private int _recursionCount = 0;

        public string Name { get { return "ParallelMergeSort"; } }

        public int GetRecursionCount() { return _recursionCount; }

        public void Sort(T[] inputOutput)
        {
            if (inputOutput == null) throw new ArgumentNullException(nameof(inputOutput));
            Sort(inputOutput, Comparer<T>.Default);
        }

        public void Sort(T[] inputOutput, IComparer<T> comparer)
        {
            if (inputOutput == null) throw new ArgumentNullException(nameof(inputOutput));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            T[] tempArray = new T[inputOutput.Length];
            MergeSort(inputOutput, tempArray, 0, inputOutput.Length - 1, comparer);
        }

        // TODO Göra något om arraySize är 1
        private void MergeSort(T[] inputOutput, T[] tempArray, int firstIndex, int lastIndex, IComparer<T> comparer)
        {
            bool arraySizeLargerThanOne = firstIndex < lastIndex;
            if (arraySizeLargerThanOne)
            {
                bool arraySmallerThanThreshold = lastIndex - firstIndex < _threshold;

                if (arraySmallerThanThreshold)
                {
                    InsertionSort(inputOutput, firstIndex, lastIndex, comparer);
                }
                else
                {
                    //_recursionCount++;
                    int middleIndex = (firstIndex + lastIndex) / 2;
                    Parallel.Invoke(
                        () =>
                        {
                            //Debug.WriteLine($"Startar parallell operation på tråd {Thread.CurrentThread.ManagedThreadId}");
                            MergeSort(inputOutput, tempArray, firstIndex, middleIndex, comparer);
                            //Debug.WriteLine($"Avslutar parallell operation på tråd {Thread.CurrentThread.ManagedThreadId}");
                        },
                        () =>
                        {
                            //Debug.WriteLine($"Startar parallell operation på tråd {Thread.CurrentThread.ManagedThreadId}");
                            MergeSort(inputOutput, tempArray, middleIndex + 1, lastIndex, comparer);
                            //Debug.WriteLine($"Avslutar parallell operation på tråd {Thread.CurrentThread.ManagedThreadId}");
                        }
                    );

                    Merge(inputOutput, tempArray, firstIndex, middleIndex, lastIndex, comparer);
                }
            }
        }

        // TODO refaktorera/clean code InsertionSort
        private void InsertionSort(T[] inputOutput, int firstIndex, int lastIndex, IComparer<T> comparer)
        {
            for (int i = firstIndex + 1; i <= lastIndex; i++)
            {
                T key = inputOutput[i];
                int j = i - 1;

                while (j >= firstIndex && comparer.Compare(inputOutput[j], key) > 0)
                {
                    inputOutput[j + 1] = inputOutput[j];
                    j--;
                }
                inputOutput[j + 1] = key;
            }
        }

        // TODO refaktorera/clean code Merge
        private void Merge(T[] inputOutput, T[] tempArray, int firstIndex, int middleIndex, int lastIndex, IComparer<T> comparer)
        {
            int i = firstIndex, j = middleIndex + 1, k = firstIndex;
            while (i <= middleIndex && j <= lastIndex)
            {
                if (comparer.Compare(inputOutput[i], inputOutput[j]) <= 0)
                {
                    tempArray[k++] = inputOutput[i++];
                }
                else
                {
                    tempArray[k++] = inputOutput[j++];
                }
            }

            while (i <= middleIndex)
            {
                tempArray[k++] = inputOutput[i++];
            }

            while (j <= lastIndex)
            {
                tempArray[k++] = inputOutput[j++];
            }

            for (i = firstIndex; i <= lastIndex; i++)
            {
                inputOutput[i] = tempArray[i];
            }
        }
    }
}
