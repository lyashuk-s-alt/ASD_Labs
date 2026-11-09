using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab22
{
    class Program
    {
        // Кількість бакетів для тестування розподілу
        private const int bucketCount = 1024;

        // 1. Легендарна хеш-функція DJB2
        static uint HashDJB2(string str)
        {
            uint hash = 5381;
            foreach (char c in str)
            {
                hash = ((hash << 5) + hash) + (uint)c;
            }
            return hash;
        }

        // 2. Хеш-функція FNV-1a
        static uint HashFNV1a(string str)
        {
            uint hash = 2166136261;
            const uint FNV_PRIME = 16777619;
            foreach (char c in str)
            {
                hash ^= (uint)c;
                hash *= FNV_PRIME;
            }
            return hash;
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Поліноміальний rolling hash та алгоритм Рабіна-Карпа
        // Повертає індекс першого входження підрядка або -1
        static int RabinKarpSearch(string text, string pattern, out int hashCollisions)
        {
            hashCollisions = 0;
            int n = text.Length;
            int m = pattern.Length;

            if (m > n) return -1;

            const uint P = 31; // Просте число для основи поліному
            uint pPow = 1;     // Значення P^(M-1) для швидкого видалення символу
            
            uint patternHash = 0;
            uint textWindowHash = 0;

            // Обчислюємо хеш для шаблону та першого вікна тексту, а також коефіцієнт pPow
            for (int i = 0; i < m; i++)
            {
                patternHash = patternHash * P + (uint)pattern[i];
                textWindowHash = textWindowHash * P + (uint)text[i];
                
                if (i < m - 1)
                {
                    pPow *= P;
                }
            }

            // Пошук за допомогою ковзного вікна
            for (int i = 0; i <= n - m; i++)
            {
                // Якщо хеші збігаються, перевіряємо посимвольно (захист від колізій)
                if (patternHash == textWindowHash)
                {
                    bool match = true;
                    for (int j = 0; j < m; j++)
                    {
                        if (text[i + j] != pattern[j])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match) return i; // Підрядок знайдено!
                    else hashCollisions++; // Сталася колізія хешів
                }

                // Обчислюємо хеш для наступного вікна (якщо ми ще не в кінці)
                if (i < n - m)
                {
                    // Формула rolling hash: видаляємо старший символ, множимо на P, додаємо новий символ
                    textWindowHash = (textWindowHash - (uint)text[i] * pPow) * P + (uint)text[i + m];
                }
            }

            return -1;
        }

        // Тестування рівномірності розподілу хешів
        static void AnalyzeHashUniformity(Func<string, uint> hashFunc, string name, List<string> words, out int outCollisions)
        {
            int[] buckets = new int[bucketCount];
            HashSet<uint> uniqueHashes = new HashSet<uint>();
            int collisions = 0;

            foreach (string word in words)
            {
                uint h = hashFunc(word);
                if (!uniqueHashes.Add(h))
                {
                    collisions++;
                }

                int bucketIdx = (int)(h % (uint)bucketCount);
                buckets[bucketIdx]++;
            }

            int emptyBuckets = 0;
            int maxInBucket = 0;
            for (int i = 0; i < bucketCount; i++)
            {
                if (buckets[i] == 0) emptyBuckets++;
                if (buckets[i] > maxInBucket) maxInBucket = buckets[i];
            }

            outCollisions = collisions;

            Console.WriteLine("\n--- Аналіз алгоритму [" + name + "] для " + words.Count + " ключів ---");
            Console.WriteLine("  Унікальних хешів: " + uniqueHashes.Count);
            Console.WriteLine("  Колізій значень:  " + collisions + " (" + ((double)collisions / words.Count * 100).ToString("F2") + "%)");
            Console.WriteLine("  Порожніх бакетів: " + emptyBuckets + " (" + ((double)emptyBuckets / bucketCount * 100).ToString("F2") + "%)");
            Console.WriteLine("  Максимум в 1 бакеті: " + maxInBucket);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №22: АНАЛІЗ КОЛІЗІЙ ТА АЛГОРИТМ РАБІНА-КАРПА        ");
            Console.WriteLine("=========================================================================");

            // 1. Генерація словника тестових слів
            List<string> dictionary = new List<string>();
            for (int i = 1; i <= 5000; i++)
            {
                dictionary.Add("student_cadet_" + i.ToString("D5"));
                dictionary.Add("sensor_temperature_node_" + i.ToString("D5"));
            }
            Console.WriteLine("Згенеровано " + dictionary.Count + " текстових ключів.");

            // Аналіз промислових хешів
            AnalyzeHashUniformity(HashDJB2, "DJB2", dictionary, out int collDJB2);
            AnalyzeHashUniformity(HashFNV1a, "FNV-1a", dictionary, out int collFNV1a);

            // 2. Тестування алгоритму Рабіна-Карпа (Варіант 2)
            Console.WriteLine("\n-------------------------------------------------------------------------");
            Console.WriteLine("Перевірка Індивідуального завдання (Варіант 2 - Рабін-Карп):");
            
            string text = "структури даних та алгоритми, пошук підрядків за допомогою хешування";
            string pattern = "алгоритми";
            
            int index = RabinKarpSearch(text, pattern, out int rkCollisions);
            
            Console.WriteLine("  Текст: \"" + text + "\"");
            Console.WriteLine("  Шаблон пошуку: \"" + pattern + "\"");
            Console.WriteLine("  Підрядок знайдено на позиції: " + index);
            Console.WriteLine("  Помилкових збігів хешів (колізій Рабіна-Карпа): " + rkCollisions);
        }
    }
}
