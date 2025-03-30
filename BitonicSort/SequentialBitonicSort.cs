namespace BitonicSort
{
    public static class SequentialBitonicSort
    {
        private static void Swap<T>(ref T i, ref T j)
        {
            (j, i) = (i, j);
        }

        private static void CompareAndSwap<T>(T[] arr, int i, int j, bool isAscending) where T : IComparable<T>
        {
            if (isAscending == (arr[i].CompareTo(arr[j]) > 0))
            {
                Swap(ref arr[i], ref arr[j]);
            }
        }

        public static void BitonicMerge<T>(T[] arr, int low, int count, bool isAscending) where T: IComparable<T>
        {
            if (count > 1)
            {
                int m = GreatestPowerOfTwoLessThan(count);

                for (int i = low; i < low + count - m; i++)
                {
                    CompareAndSwap(arr, i, i + m, isAscending);
                }

                BitonicMerge(arr, low, m, isAscending);
                BitonicMerge(arr, low + m, count - m, isAscending);
            }
        }

        private static int GreatestPowerOfTwoLessThan(int n)
        {
            int k = 1;
            while (k > 0 && k < n) k <<= 1;
            return k >> 1;
        }

        public static void Sort<T>(T[] arr, int low, int count, bool isAscending) where T : IComparable<T>
        {
            if (count > 1)
            {
                int m = count / 2;
                Sort(arr, low, m, !isAscending);
                Sort(arr, low + m, count - m, isAscending);
                BitonicMerge(arr, low, count, isAscending);
            }
        }

        public static void PrintArray(int[] arr)
        {
            foreach (int val in arr)
            {
                Console.WriteLine(val + " ");
            }
            Console.WriteLine();
        }

        public static bool IsSorted<T>(T[] arr, bool ascending = true) where T : IComparable<T>
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (ascending && arr[i].CompareTo(arr[i + 1]) > 0) return false;
                if (!ascending && arr[i].CompareTo(arr[i + 1]) < 0) return false;
            }
            return true;
        }

        public static void Sort<T>(T[] arr, bool isAscending = true) where T : IComparable<T>
        {
            Sort(arr, 0, arr.Length, isAscending);
        }
    }
}
