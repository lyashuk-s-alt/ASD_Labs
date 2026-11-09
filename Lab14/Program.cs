using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab14
{
    class Program
    {
        // Оптимизированный Bubble Sort с запоминанием индекса последнего обмена
        static void OptimizedBubbleSort(int[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            int n = arr.Length;
            
            // Граница, до которой нужно выполнять проверки
            int lastExchangeIndex = n - 1;

            while (lastExchangeIndex > 0)
            {
                int currentPassLastExchange = 0;

                for (int j = 0; j < lastExchangeIndex; j++)
                {
                    comparisons++;
                    if (arr[j] > arr[j + 1])
                    {
                        // Обмен соседних элементов
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;

                        swaps++;
                        // Запоминаем индекс текущего обмена
                        currentPassLastExchange = j;
                    }
                }

                // Следующий проход будет только до места последнего обмена
                lastExchangeIndex = currentPassLastExchange;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №14: BUBBLE SORT OPTIMIZED (VAR 2)     ");
            Console.WriteLine("=========================================================================");

            int n = 1000;
            int[] randomData = new int[n];
            int[] sortedData = new int[n];
            int[] reversedData = new int[n];

            Random rnd = new Random(42);
            for (int i = 0; i < n; i++)
            {
                randomData[i] = rnd.Next(1, 10000);
                sortedData[i] = i;
                reversedData[i] = n - i;
            }

            // 1. Тест на случайных данных
            int[] test1 = (int[])randomData.Clone();
            Stopwatch sw = Stopwatch.StartNew();
            OptimizedBubbleSort(test1, out long comp1, out long swap1);
            sw.Stop();
            Console.WriteLine($"\n 1. Випадковий масив (N={n}):");
            Console.WriteLine($"   Порівнянь: {comp1} | Перестановок: {swap1} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // 2. Тест на уже отсортированном массиве (Лучший случай O(n))
            int[] test2 = (int[])sortedData.Clone();
            sw.Restart();
            OptimizedBubbleSort(test2, out long comp2, out long swap2);
            sw.Stop();
            Console.WriteLine($"\n 2. Вже відсортований масив (Найкращий випадок):");
            Console.WriteLine($"   Порівнянь: {comp2} | Перестановок: {swap2} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            // 3. Тест на обратно отсортированном массиве (Худший случай O(n^2))
            int[] test3 = (int[])reversedData.Clone();
            sw.Restart();
            OptimizedBubbleSort(test3, out long comp3, out long swap3);
            sw.Stop();
            Console.WriteLine($"\n 3. Зворотно впорядкований масив (Найгірший випадок):");
            Console.WriteLine($"   Порівнянь: {comp3} | Перестановок: {swap3} | Час: {sw.Elapsed.TotalMilliseconds:F4} ms");

            Console.WriteLine("\n Висновок: запам'ятовування індексу обміну значно скорочує кількість порівнянь у частково впорядкованих масивах!");
        }
    }
}
