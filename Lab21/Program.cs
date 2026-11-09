using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab21
{
    class Program
    {
        // 1. Інтерполяційний пошук O(log log n) для рівномірно розподілених даних
        static int InterpolationSearch(int[] arr, int target, out int iterations)
        {
            iterations = 0;
            int low = 0;
            int high = arr.Length - 1;

            while (low <= high && target >= arr[low] && target <= arr[high])
            {
                iterations++;
                if (low == high)
                {
                    if (arr[low] == target) return low;
                    return -1;
                }

                // Пропорційна оцінка положення ключа
                int pos = low + (int)(((long)(target - arr[low]) * (high - low)) / (arr[high] - arr[low]));

                if (arr[pos] == target) return pos;

                if (arr[pos] < target)
                    low = pos + 1;
                else
                    high = pos - 1;
            }
            return -1;
        }

        // 2. Експоненційний пошук O(log n)
        static int ExponentialSearch(int[] arr, int target, out int steps)
        {
            steps = 1;
            if (arr[0] == target) return 0;

            int i = 1;
            while (i < arr.Length && arr[i] <= target)
            {
                steps++;
                i *= 2;
            }

            int left = i / 2;
            int right = Math.Min(i, arr.Length - 1);

            // Бінарний пошук у знайденому діапазоні
            int low = left, high = right;
            while (low <= high)
            {
                steps++;
                int mid = low + (high - low) / 2;
                if (arr[mid] == target) return mid;
                if (arr[mid] < target) low = mid + 1;
                else high = mid - 1;
            }
            return -1;
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Пошук Фібоначчі (Fibonacci Search) за золотим перерізом
        static int FibonacciSearch(int[] arr, int target, out int iterations)
        {
            iterations = 0;
            int n = arr.Length;

            // Ініціалізація чисел Фібоначчі
            int fibM2 = 0; // (m-2)-е число Фібоначчі
            int fibM1 = 1; // (m-1)-е число Фібоначчі
            int fibM = fibM2 + fibM1; // m-е число Фібоначчі

            // Знаходимо найменше число Фібоначчі, яке більше або дорівнює n
            while (fibM < n)
            {
                fibM2 = fibM1;
                fibM1 = fibM;
                fibM = fibM2 + fibM1;
            }

            // Зсув від початку масиву
            int offset = -1;

            while (fibM > 1)
            {
                iterations++;
                // Визначаємо індекс для перевірки, обмежуючи його межами масиву
                int i = Math.Min(offset + fibM2, n - 1);

                if (arr[i] < target)
                {
                    // Шукане значення більше, зміщуємо діапазон праворуч
                    fibM = fibM1;
                    fibM1 = fibM2;
                    fibM2 = fibM - fibM1;
                    offset = i;
                }
                else if (arr[i] > target)
                {
                    // Шукане значення менше, звужуємо діапазон ліворуч
                    fibM = fibM2;
                    fibM1 = fibM1 - fibM2;
                    fibM2 = fibM - fibM1;
                }
                else
                {
                    // Елемент знайдено
                    return i;
                }
            }

            // Перевірка останнього елемента
            if (fibM1 == 1 && offset + 1 < n && arr[offset + 1] == target)
            {
                iterations++;
                return offset + 1;
            }

            return -1;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №21: ІНТЕРПОЛЯЦІЙНИЙ, ЕКСПONЕНЦІЙНИЙ ТА ФІБОНАЧЧІ    ");
            Console.WriteLine("=========================================================================");

            int n = 1000000; // Масив на 1 мільйон елементів
            int[] data = new int[n];
            for (int i = 0; i < n; i++) data[i] = i * 3; // Рівномірний розподіл чисел

            // Елемент ближче до кінця масиву
            int target = 2850000; 

            Console.WriteLine("Масив: N = 1,000,000 рівномірно розподілених чисел");
            Console.WriteLine("Шукане значення: " + target + "\n");

            // --- 1. Інтерполяційний пошук ---
            Stopwatch sw = Stopwatch.StartNew();
            int idx1 = InterpolationSearch(data, target, out int iter1);
            sw.Stop();
            Console.WriteLine("[Interpolation Search]");
            Console.WriteLine("  Індекс: " + idx1 + " | Кроків: " + iter1 + " | Час: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");

            // --- 2. Експоненційний пошук ---
            sw.Restart();
            int idx2 = ExponentialSearch(data, target, out int iter2);
            sw.Stop();
            Console.WriteLine("\n[Exponential Search]");
            Console.WriteLine("  Індекс: " + idx2 + " | Кроків: " + iter2 + " | Час: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");

            // --- 3. Пошук Фібоначчі (Варіант 2) ---
            sw.Restart();
            int idx3 = FibonacciSearch(data, target, out int iter3);
            sw.Stop();
            Console.WriteLine("\n[Fibonacci Search (Варіант 2)]");
            Console.WriteLine("  Індекс: " + idx3 + " | Кроків: " + iter3 + " | Час: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");
        }
    }
}
