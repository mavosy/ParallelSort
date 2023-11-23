using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sorting
{
    //klassen kräver att typen T ska vara jämförbar med sig själv
    public class ParallelMergeSort<T> : ISort<T> where T : IComparable<T>
    {
        // Sätter ett gränsvärde för storleken på arrayen.
        // Vid små arrayer ska InsertionSort användas istället för MergeSort, för att undvika deoptimisering.
        // Värdet är delvis optimerat efter testning, men bara testad på PC med 8 logiska processorer.
        // Alternativt låter man storleken på arrayen inputOutput bestämma värdet på _thresholdSortAlgorithmChange.

        private readonly int _thresholdSortAlgorithmChange = 80;

        public string Name { get { return "ParallelMergeSort"; } }

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
            int firstIndex = 0;
            int lastIndex = inputOutput.Length - 1;
            MergeSort(inputOutput, tempArray, firstIndex, lastIndex, comparer);
        }

        private void MergeSort(T[] inputOutput, T[] tempArray, int firstIndex, int lastIndex, IComparer<T> comparer)
        {
            bool arraySizeLargerThanOne = firstIndex < lastIndex;
            if (arraySizeLargerThanOne)
            {
                bool arraySmallerThanThreshold = lastIndex - firstIndex < _thresholdSortAlgorithmChange;
                if (arraySmallerThanThreshold)
                {
                    InsertionSort(inputOutput, firstIndex, lastIndex, comparer);
                }
                else
                {
                    int middleIndex = (firstIndex + lastIndex) / 2;
                    Parallel.Invoke(
                        () => MergeSort(inputOutput, tempArray, firstIndex, middleIndex, comparer),
                        () => MergeSort(inputOutput, tempArray, middleIndex + 1, lastIndex, comparer)
                    );

                    Merge(inputOutput, tempArray, firstIndex, middleIndex, lastIndex, comparer);
                }
            }
        }

        private void InsertionSort(T[] inputOutput, int firstIndex, int lastIndex, IComparer<T> comparer)
        {
            for (int currentIndex = firstIndex + 1; currentIndex <= lastIndex; currentIndex++)
            {
                T key = inputOutput[currentIndex];
                int compareIndex = currentIndex - 1;

                while (compareIndex >= firstIndex && comparer.Compare(inputOutput[compareIndex], key) > 0)
                {
                    inputOutput[compareIndex + 1] = inputOutput[compareIndex];
                    compareIndex--;
                }
                inputOutput[compareIndex + 1] = key;
            }
        }

        private void Merge(T[] inputOutput, T[] tempArray, int firstIndex, int middleIndex, int lastIndex, IComparer<T> comparer)
        {
            int firstHalfIndex = firstIndex, secondHalfIndex = middleIndex + 1, mergedIndex = firstIndex;
            while (firstHalfIndex <= middleIndex && secondHalfIndex <= lastIndex)
            {
                bool firstIsLessOrEqualToLast = comparer.Compare(inputOutput[firstHalfIndex], inputOutput[secondHalfIndex]) <= 0;
                if (firstIsLessOrEqualToLast)
                {
                    tempArray[mergedIndex++] = inputOutput[firstHalfIndex++];
                }
                else
                {
                    tempArray[mergedIndex++] = inputOutput[secondHalfIndex++];
                }
            }

            CopyRemainingElementsFromArraySections(inputOutput, tempArray, firstHalfIndex, middleIndex, mergedIndex);
            CopyRemainingElementsFromArraySections(inputOutput, tempArray, secondHalfIndex, lastIndex, mergedIndex);
            Array.Copy(tempArray, firstIndex, inputOutput, firstIndex, lastIndex - firstIndex + 1);
        }

        private void CopyRemainingElementsFromArraySections(T[] sourceArray, T[] destinationArray, int sourceArrayStart, int sourceArrayEnd, int destinationIndex)
        {
            while (sourceArrayStart <= sourceArrayEnd)
            {
                destinationArray[destinationIndex++] = sourceArray[sourceArrayStart++];
            }
        }
    }
}