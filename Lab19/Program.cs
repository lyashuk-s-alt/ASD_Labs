using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab19
{
    class Program
    {
        // Допоміжне сортування підрахунком за конкретним індексом символу в слові
        static void CountSortByChar(string[] arr, int charIdx)
        {
            int n = arr.Length;
            string[] output = new string[n];
            
            // Розмір 256 для покриття базових символів ASCII
            int[] count = new int[256];

            // Підрахунок кількості кожного символу на позиції charIdx
            for (int i = 0; i < n; i++)
            {
                int charValue = arr[i][charIdx];
                count[charValue]++;
            }

            // Перетворення count[i] так, щоб він містив реальну позицію у вихідному масиві
            for (int i = 1; i < 256; i++)
            {
                count[i] += count[i - 1];
            }

            // Побудова вихідного масиву з кінця для збереження стабільності сортування
            for (int i = n - 1; i >= 0; i--)
            {
                int charValue = arr[i][charIdx];
                output[count[charValue] - 1] = arr[i];
                count[charValue]--;
            }

            // Копіювання результату назад в оригінальний масив
            for (int i = 0; i < n; i++)
            {
                arr[i] = output[i];
            }
        }

        // Рядковий Порозрядний Radix Sort (LSD) для слів однакової довжини
        static void StringRadixSort(string[] arr)
        {
            if (arr.Length == 0) return;

            // Оскільки за умовою всі слова однакової довжини, беремо довжину першого слова
            int wordLength = arr[0].Length;

            // Проходимо по всіх розрядах (символах) справа наліво (від кінця слова до початку)
            for (int charIdx = wordLength - 1; charIdx >= 0; charIdx--)
            {
                CountSortByChar(arr, charIdx);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №19: РЯДКОВИЙ RADIX SORT (ВАРАНТ 2)    ");
            Console.WriteLine("=========================================================================");

            // Масив слів однакової довжини (кожен рядок має рівно 5 символів)
            string[] words = new string[] {
                "apple",
                "zebra",
                "coder",
                "brain",
                "alpha",
                "basic",
                "world",
                "cloud",
                "pixel",
                "logic"
            };

            Console.WriteLine("Вхідний масив слів:");
            foreach (string word in words)
            {
                Console.Write(word + " ");
            }
            Console.WriteLine("\n");

            Stopwatch sw = Stopwatch.StartNew();
            StringRadixSort(words);
            sw.Stop();

            Console.WriteLine("Масив після рядкового Radix Sort (за алфавітом):");
            foreach (string word in words)
            {
                Console.Write(word + " ");
            }
            Console.WriteLine("\n");

            Console.WriteLine("⏱️ Час виконання сортування: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");

            // Перевірка правильності впорядкування
            bool isSorted = true;
            for (int i = 0; i < words.Length - 1; i++)
            {
                if (string.Compare(words[i], words[i + 1]) > 0)
                {
                    isSorted = false;
                    break;
                }
            }
            Console.WriteLine("Результат: Масив відсортовано без помилок: " + isSorted);
        }
    }
}
