using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab17
{
    class Program
    {
        // Допоміжний метод для обміну елементів
        static void Swap(int[] arr, int a, int b, ref long swaps)
        {
            if (a == b) return;
            int temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
            swaps++;
        }

        // Стратегія "Медіана з трьох" (Median of Three)
        // Знаходить медіану серед першого, середнього та останнього елементів і ставить її в кінець (high)
        static void MedianOfThree(int[] arr, int low, int high, ref long comparisons, ref long swaps)
        {
            int mid = low + (high - low) / 2;

            // Сортуємо три елементи: arr[low], arr[mid], arr[high] між собою
            comparisons++;
            if (arr[low] > arr[mid]) 
                Swap(arr, low, mid, ref swaps);

            comparisons++;
            if (arr[low] > arr[high]) 
                Swap(arr, low, high, ref swaps);

            comparisons++;
            if (arr[mid] > arr[high]) 
                Swap(arr, mid, high, ref swaps);

            // Тепер медіана знаходиться в arr[mid].
            // Переносимо її в кінець масиву (high), щоб схема Ломуто використовувала її як опорний елемент (pivot)
            Swap(arr, mid, high, ref swaps);
        }

        // Схема розбиття Ломуто із інтегрованим вибором медіани
        static int LomutoPartition(int[] arr, int low, int high, ref long comparisons, ref long swaps)
        {
            // Викликаємо оптимізацію перед вибором опорного елемента
            MedianOfThree(arr, low, high, ref comparisons, ref swaps);

            int pivot = arr[high]; // Опорний елемент тепер є медіаною
            int i = low - 1;       // Індекс менших елементів

            for (int j = low; j < high; j++)
            {
                comparisons++;
                if (arr[j] <= pivot)
                {
                    i++;
                    Swap(arr, i, j, ref swaps);
                }
            }

            // Ставимо pivot на його правильне фінальне місце (i + 1)
            Swap(arr, i + 1, high, ref swaps);
            return i + 1;
        }

        // Рекурсивне сортування QuickSort
        static void QuickSort(int[] arr, int low, int high, ref long comparisons, ref long swaps)
        {
            if (low < high)
            {
                int pi = LomutoPartition(arr, low, high, ref comparisons, ref swaps);

                // Рекурсивно сортуємо ліву та праву частини
                QuickSort(arr, low, pi - 1, ref comparisons, ref swaps);
                QuickSort(arr, pi + 1, high, ref comparisons, ref swaps);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №17: QUICKSORT (MEDIAN OF THREE)      ");
            Console.WriteLine("=========================================================================");

            int n = 20000; // Тестуємо на 20 тисячах елементів відповідно до вказівки
            int[] randomData = new int[n];
            int[] sortedData = new int[n];

            Random rand = new Random(42);
            for (int i = 0; i < n; i++)
            {
                randomData[i] = rand.Next(1, 100000);
                sortedData[i] = i; // Масив для перевірки деградації (вже відсортований)
            }

            // ТЕСТ 1: Сортування випадкового масиву
            int[] test1 = (int[])randomData.Clone();
            long comp1 = 0, swap1 = 0;
            Stopwatch sw = Stopwatch.StartNew();
            QuickSort(test1, 0, test1.Length - 1, ref comp1, ref swap1);
            sw.Stop();

            Console.WriteLine("\n 1. Сортування випадкового масиву (N=" + n + "):");
            Console.WriteLine("   Час сортування: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");
            Console.WriteLine("   Кількість порівнянь: " + comp1);
            Console.WriteLine("   Кількість обмінів: " + swap1);

            // ТЕСТ 2: Сортування вже відсортованого масиву (Запобігання деградації O(n^2))
            int[] test2 = (int[])sortedData.Clone();
            long comp2 = 0, swap2 = 0;
            sw.Restart();
            QuickSort(test2, 0, test2.Length - 1, ref comp2, ref swap2);
            sw.Stop();

            Console.WriteLine("\n 2. Сортування вже впорядкованого масиву (Уникнення гіршого випадку):");
            Console.WriteLine("   Час сортування: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");
            Console.WriteLine("   Кількість порівнянь: " + comp2);
            Console.WriteLine("   Кількість обмінів: " + swap2);
        }
    }
}
