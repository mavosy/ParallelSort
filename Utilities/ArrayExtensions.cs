using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class ArrayExtensions
    {
        public static int[] CreateOrderedArray(int numberOfItems)
        {
            return Enumerable.Range(1, numberOfItems).ToArray();
        }

        public static int[] CreateReverseOrderedArray(int numberOfItems)
        {
            int[] numbers = CreateOrderedArray(numberOfItems);
            for (int i = 0; i < numberOfItems / 2; i++) {
                int tmp = numbers[i];
                numbers[i] = numbers[numberOfItems - 1 - i];
                numbers[numberOfItems - 1 - i] = tmp;
            }
            return numbers;
        }

        public static int[] CreateRandomArray(int numberOfItems, Random rng)
        {
            int[] numbers = CreateOrderedArray(numberOfItems);
            rng.Shuffle<int>(numbers);
            return numbers;
        }

        public static MyKeyValue<int, string>[] CreateOrderedMKVArray(int numberOfItems)
        {
            MyKeyValue<int, string>[] array = new MyKeyValue<int, string>[numberOfItems];
            foreach (int k in Enumerable.Range(1, numberOfItems)) {
                array[k-1] = new MyKeyValue<int, string>(k, "");
            }
            return array;
        }

        public static MyKeyValue<int, string>[] CreateReverseOrderedMKVArray(int numberOfItems)
        {
            MyKeyValue<int, string>[] numbers = CreateOrderedMKVArray(numberOfItems);
            for (int i = 0; i < numberOfItems / 2; i++) {
                MyKeyValue<int, string> tmp = numbers[i];
                numbers[i] = numbers[numberOfItems - 1 - i];
                numbers[numberOfItems - 1 - i] = tmp;
            }
            return numbers;
        }

        public static MyKeyValue<int, string>[] CreateRandomMKVArray(int numberOfItems, Random rng)
        {
            MyKeyValue<int, string>[] numbers = CreateOrderedMKVArray(numberOfItems);
            rng.Shuffle<MyKeyValue<int, string>>(numbers);
            return numbers;
        }

        public static bool VerifySorted<T>(T[] input)
        {
            return VerifySorted<T>(input, Comparer<T>.Default);
        }

        public static bool VerifySorted<T>(T[] input, IComparer<T> comparer)
        {
            for (int i = 0; i < input.Length - 1; i++) {
                if (comparer.Compare(input[i], input[i+1]) > 0) {
                    return false;
                }
            }
            return true;
        }

        public static bool VerifySortedOneToLength(MyKeyValue<int, string>[] input, int length)
        {
            bool ok = input.Length == length;
            for (int i = 1; i < input.Length; i++) {
                ok = ok && (input[i - 1].Key + 1 == input[i].Key);
            }
            return ok;
        }
    }
}
