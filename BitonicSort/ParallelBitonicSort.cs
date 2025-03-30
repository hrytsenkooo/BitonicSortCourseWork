namespace BitonicSort
{
    public class ParallelBitonicSort
    {
        private static int ParallelizationThreshold;

        public static void SetParallelizationThreshold(int threshold)
        {
            ParallelizationThreshold = threshold;
        }

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

        private static void BitonicMerge<T>(T[] arr, int low, int count, bool isAscending, int maxDepth) where T : IComparable<T>
        {
            if (count <= 1) return;
            int m = GreatestPowerOfTwoLessThan(count);
            if (maxDepth > 0 && count > ParallelizationThreshold)
            {
                Parallel.For(low, low + count - m, i =>
                {
                    CompareAndSwap(arr, i, i + m, isAscending);
                });

                Parallel.Invoke(
                    () => BitonicMerge(arr, low, m, isAscending, maxDepth - 1),
                    () => BitonicMerge(arr, low + m, count - m, isAscending, maxDepth - 1)
                );
            }
            else
            {
                for (int i = low; i < low + count - m; i++)
                {
                    CompareAndSwap(arr, i, i + m, isAscending);
                }

                BitonicMerge(arr, low, m, isAscending, 0);
                BitonicMerge(arr, low + m, count - m, isAscending, 0);
            }
        }

        public static void Sort<T>(T[] arr, int low, int count, bool isAscending, int maxDepth) where T : IComparable<T>
        {
            if (count <= 1) return;

            if (maxDepth > 0 && count > ParallelizationThreshold)
            {
                int m = count / 2;
                Parallel.Invoke(
                    () => Sort(arr, low, m, !isAscending, maxDepth - 1),
                    () => Sort(arr, low + m, count - m, isAscending, maxDepth - 1)
                );
                BitonicMerge(arr, low, count, isAscending, maxDepth - 1);
            }
            else
            {
                int m = count / 2;
                Sort(arr, low, m, !isAscending, 0);
                Sort(arr, low + m, count - m, isAscending, 0);
                BitonicMerge(arr, low, count, isAscending, 0);
            }
        }

        private static int GreatestPowerOfTwoLessThan(int n)
        {
            int k = 1;
            while (k > 0 && k < n) k <<= 1;
            return k >> 1;
        }

        public static void Sort<T>(T[] arr, bool isAscending = true, int maxDepth = 3) where T : IComparable<T>
        {
            Sort(arr, 0, arr.Length, isAscending, maxDepth);
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
    }
}
