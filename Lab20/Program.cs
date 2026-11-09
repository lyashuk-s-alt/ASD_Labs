using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab20
{
    class Program
    {
        // 1. Лінійний пошук O(n) для порівняння метрик
        static int LinearSearch(int[] arr, int target, out int iterations)
        {
            iterations = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                iterations++;
                if (arr[i] == target) return i;
            }
            return -1;
        }

        // 2. ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Бінарний пошук у циклічно зсунутому масиві - O(log n)
        static int RotatedBinarySearch(int[] arr, int target, out int iterations)
        {
            iterations = 0;
            int low = 0;
            int high = arr.Length - 1;

            while (low <= high)
            {
                iterations++;
                int mid = low + (high - low) / 2;

                if (arr[mid] == target) return mid;

                // Перевіряємо, чи ліва частина [low..mid] відсортована нормальним чином
                if (arr[low] <= arr[mid])
                {
                    // Якщо target лежить у діапазоні лівої відсортованої частини
                    if (target >= arr[low] && target < arr[mid])
                    {
                        high = mid - 1; // Шукаємо ліворуч
                    }
                    else
                    {
                        low = mid + 1;  // Шукаємо праворуч
                    }
                }
                // Інакше відсортованою є права частина [mid..high]
                else
                {
                    // Якщо target лежить у діапазоні правої відсортованої частини
                    if (target > arr[mid] && target <= arr[high])
                    {
                        low = mid + 1;  // Шукаємо праворуч
                    }
                    else
                    {
                        high = mid - 1; // Шукаємо ліворуч
                    }
                }
            }
            return -1;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №20: ROTATED BINARY SEARCH (VAR 2)     ");
            Console.WriteLine("=========================================================================");

            int n = 100000; // Розмірність масиву (100 тисяч елементів)
            int[] data = new int[n];
            
            // 1. Спочатку створюємо звичайний відсортований масив парних чисел
            for (int i = 0; i < n; i++) data[i] = i * 2;

            // 2. Робимо циклічний зсув масиву (наприклад, на 30000 елементів ліворуч)
            int shiftOffset = 30000;
            int[] rotatedData = new int[n];
            for (int i = 0; i < n; i++)
            {
                rotatedData[(i - shiftOffset + n) % n] = data[i];
            }

            // Задаємо шукане число, яке знаходиться ближче до кінця логічного масиву
            int target = 195432; 

            Console.WriteLine("Розмірність зсунутого масиву: N = " + n);
            Console.WriteLine("Шукане значення: " + target + "\n");

            // --- ТЕСТ 1: Лінійний пошук ---
            Stopwatch sw = Stopwatch.StartNew();
            int idx1 = LinearSearch(rotatedData, target, out int iter1);
            sw.Stop();
            Console.WriteLine("[Лінійний пошук O(n)]");
            Console.WriteLine("  Знайдено індекс: " + idx1 + " | Ітерацій: " + iter1 + " | Час: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");

            // --- ТЕСТ 2: Оптимізований бінарний пошук ---
            sw.Restart();
            int idx2 = RotatedBinarySearch(rotatedData, target, out int iter2);
            sw.Stop();
            Console.WriteLine("\n[Модифікований бінарний пошук O(log n)]");
            Console.WriteLine("  Знайдено індекс: " + idx2 + " | Ітерацій: " + iter2 + " | Час: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");

            // Розрахунок ефективності
            double acceleration = (double)iter1 / iter2;
            Console.WriteLine("\nПрискорення за кількістю кроків у: " + acceleration.ToString("F2") + " разів!");
        }
    }
}
