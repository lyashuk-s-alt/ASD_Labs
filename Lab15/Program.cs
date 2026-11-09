using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab15
{
    // Класс объекта для тестирования стабильности сортировки
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    class Program
    {
        // 1. Selection Sort: стабильность зависит от реализации, классическая — НЕСТАБИЛЬНАЯ
        static void SelectionSort(User[] arr, out long comparisons, out long swaps)
        {
            comparisons = 0;
            swaps = 0;
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    comparisons++;
                    if (arr[j].Id < arr[minIdx].Id)
                    {
                        minIdx = j;
                    }
                }

                if (minIdx != i)
                {
                    User temp = arr[i];
                    arr[i] = arr[minIdx];
                    arr[minIdx] = temp;
                    swaps++;
                }
            }
        }

        // 2. Insertion Sort: СТАБИЛЬНАЯ сортировка (так как элемент не перескакивает равные)
        static void InsertionSort(User[] arr, out long comparisons, out long shifts)
        {
            comparisons = 0;
            shifts = 0;
            int n = arr.Length;

            for (int i = 1; i < n; i++)
            {
                User key = arr[i];
                int j = i - 1;

                while (j >= 0)
                {
                    comparisons++;
                    if (arr[j].Id > key.Id) // Строгое условие '>' обеспечивает стабильность
                    {
                        arr[j + 1] = arr[j];
                        shifts++;
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }
                arr[j + 1] = key;
            }
        }

        static void PrintArray(User[] arr)
        {
            foreach (var user in arr)
            {
                Console.Write($"[{user.Id}: {user.Name}] ");
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №15: ДОСЛІДЖЕННЯ СТАБІЛЬНОСТІ СОРТУВАНЬ ");
            Console.WriteLine("=========================================================================");

            // Создаем тестовый массив с одинаковыми ключами Id. 
            // Исходный порядок для Id=2: сначала "Вася", затем "Петя".
            User[] original = new User[]
            {
                new User(3, "Аня"),
                new User(2, "Вася"), // Первый элемент с Id = 2
                new User(1, "Оля"),
                new User(2, "Петя")  // Второй элемент с Id = 2
            };

            Console.WriteLine("Вихідний масив:");
            PrintArray(original);

            // Тестируем Selection Sort
            User[] arr1 = new User[original.Length];
            Array.Copy(original, arr1, original.Length);
            SelectionSort(arr1, out long c1, out long s1);

            Console.WriteLine("\nПісля Selection Sort:");
            PrintArray(arr1);
            Console.WriteLine($"Порівнянь: {c1}, Обмінів: {s1}");

            // Тестируем Insertion Sort
            User[] arr2 = new User[original.Length];
            Array.Copy(original, arr2, original.Length);
            InsertionSort(arr2, out long c2, out long s2);

            Console.WriteLine("\nПісля Insertion Sort:");
            PrintArray(arr2);
            Console.WriteLine($"Порівнянь: {c2}, Зсувів: {s2}");
        }
    }
}
