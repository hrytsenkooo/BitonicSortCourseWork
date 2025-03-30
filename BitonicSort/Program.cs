using BitonicSort;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        //int[] sizes = { 100, 1_000, 10_000, 100_000, 1_000_000, 10_000_000 }; 
        //int[] thresholds = { 512, 1024, 4096, 8192, 16384 }; 
        //int[] depths = { 1, 2, 3, 4, 5, 6 }; 

        //foreach (int size in sizes)
        //{
        //    int[] originalArray = GenerateRandomArray(size);

        //    Console.WriteLine($"\n--- Sorting {size} elements ---");

        //    foreach (int threshold in thresholds)
        //    {
        //        ParallelBitonicSort.SetParallelizationThreshold(threshold);

        //        foreach (int depth in depths)
        //        {
        //            int[] testArray = (int[])originalArray.Clone();
        //            Stopwatch sw = Stopwatch.StartNew();
        //            ParallelBitonicSort.Sort(testArray, true, depth);
        //            sw.Stop();

        //            Console.WriteLine($"Threshold={threshold}, Depth={depth}, Time={sw.Elapsed.TotalMilliseconds} ms");
        //        }
        //    }
        //}

        int[] sizes = new int[] { 100, 1_000, 10_000, 100_000, 1_000_000, 10_000_000, 100_000_000 };
        foreach (var size in sizes)
        {
            Console.WriteLine($"Test for array size {size}:");
            TestOptimalParameters(size);
        }
    }

    static int[] GenerateRandomArray(int size)
    {
        Random rand = new Random();
        return Enumerable.Range(0, size).Select(_ => rand.Next(1000000)).ToArray();
    }

    public static void TestParallelSorting(int size)
    {
        Random random = new Random();
        int[] array = new int[size];
        int threshold = 4096; 
        int depth = 4; 
        double totalTime = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < size; j++)
            {
                array[j] = random.Next(int.MinValue, int.MaxValue);
            }

            ParallelBitonicSort.SetParallelizationThreshold(threshold);
            Stopwatch stopwatch = Stopwatch.StartNew();
            ParallelBitonicSort.Sort(array, true, depth);
            stopwatch.Stop();
            totalTime += stopwatch.Elapsed.TotalMilliseconds;

            if (!ParallelBitonicSort.IsSorted(array, true))
            {
                Console.WriteLine("Error! Array is not sorted.");
                return;
            }
            else
            {
                Console.WriteLine($"Sorting {i + 1} for size {size} completed successfully.");
            }
        }

        double averageTime = totalTime / 3;
        Console.WriteLine($"Average sorting time for an array of size {size}: {averageTime:F2} ms");
        Console.WriteLine(new string('-', 40));
    }

    public static void TestSorting(int size)
    {
        Random random = new Random();
        int[] array = new int[size];

        double totalTime = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < size; j++)
            {
                array[j] = random.Next(int.MinValue, int.MaxValue);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            SequentialBitonicSort.Sort(array, true);
            stopwatch.Stop();
            totalTime += stopwatch.Elapsed.TotalMilliseconds;

            if (!SequentialBitonicSort.IsSorted(array, true))
            {
                Console.WriteLine("Error! Array is not sorted.");
                return;
            }
            else
            {
                Console.WriteLine($"Sorting {i + 1} for size {size} completed successfully.");
            }
        }

        double averageTime = totalTime / 3;
        Console.WriteLine($"Average sorting time for an array of size {size}: {averageTime:F2} мс");
        Console.WriteLine(new string('-', 40));
    }

    public static void TestOptimalParameters(int size)
    {
        Random random = new Random();
        int[] array = new int[size];
        int bestThreshold = 0;
        int bestDepth = 0;
        double bestTime = double.MaxValue;

        int[] thresholds = { 4096, 8192, 16384 };
        int[] depths = { 3, 4, 5, 6 };

        foreach (int threshold in thresholds)
        {
            foreach (int depth in depths)
            {
                ParallelBitonicSort.SetParallelizationThreshold(threshold);
                double totalTime = 0;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        array[j] = random.Next(int.MinValue, int.MaxValue);
                    }

                    Stopwatch stopwatch = Stopwatch.StartNew();
                    ParallelBitonicSort.Sort(array, true, depth);
                    stopwatch.Stop();
                    totalTime += stopwatch.Elapsed.TotalMilliseconds;

                    if (!ParallelBitonicSort.IsSorted(array, true))
                    {
                        Console.WriteLine($"Error! Array of size {size} is not sorted with Threshold={threshold}, Depth={depth}");
                        return;
                    }
                }

                double averageTime = totalTime / 3;

                if (averageTime < bestTime)
                {
                    bestTime = averageTime;
                    bestThreshold = threshold;
                    bestDepth = depth;
                }
            }
        }

        Console.WriteLine($"Best parameters for size {size}: Threshold = {bestThreshold}, Depth = {bestDepth}, Time = {bestTime:F2} ms");
        Console.WriteLine(new string('-', 50));
    }
}

