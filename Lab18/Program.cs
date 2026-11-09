using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab18
{
    // Клас студента для демонстрації стабільності сортування
    public class Student
    {
        public string Name { get; set; }
        public int Grade { get; set; }

        public Student(string name, int grade)
        {
            Name = name;
            Grade = grade;
        }
    }

    class Program
    {
        // 1. Злиття двох відсортованих частин [left..mid] та [mid+1..right]
        static void Merge(Student[] arr, int left, int mid, int right, Student[] temp)
        {
            int i = left;      // Початок лівої частини
            int j = mid + 1;   // Початок правої частини
            int k = left;      // Індекс у допоміжному масиві

            while (i <= mid && j <= right)
            {
                // Умова '<=' критично важлива: вона забезпечує стабільність алгоритму
                if (arr[i].Grade <= arr[j].Grade)
                {
                    temp[k++] = arr[i++];
                }
                else
                {
                    temp[k++] = arr[j++];
                }
            }

            // Дописуємо залишки лівої частини (якщо є)
            while (i <= mid) temp[k++] = arr[i++];

            // Дописуємо залишки правої частини (якщо є)
            while (j <= right) temp[k++] = arr[j++];

            // Копіюємо відсортовану ділянку назад в оригінальний масив
            for (int idx = left; idx <= right; idx++)
            {
                arr[idx] = temp[idx];
            }
        }

        // 2. Рекурсивне розбиття масиву навпіл
        static void MergeSort(Student[] arr, int left, int right, Student[] temp)
        {
            if (left < right)
            {
                int mid = left + (right - left) / 2;

                MergeSort(arr, left, mid, temp);
                MergeSort(arr, mid + 1, right, temp);

                Merge(arr, left, mid, right, temp);
            }
        }

        static void PrintArray(Student[] arr)
        {
            foreach (var student in arr)
            {
                Console.Write("[" + student.Name + ": " + student.Grade + "] ");
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №18: MERGESORT (СТАБІЛЬНІСТЬ ОБ'ЄКТІВ) ");
            Console.WriteLine("=========================================================================");

            // Створюємо масив студентів з однаковими оцінками (Grade) для перевірки стабільності.
            // Початковий порядок для оцінки 90: спочатку "Іван", потім "Марія".
            Student[] data = new Student[]
            {
                new Student("Олег", 75),
                new Student("Іван", 90),   // Перший студент з оцінкою 90
                new Student("Анна", 82),
                new Student("Марія", 90)   // Другий студент з оцінкою 90
            };

            Console.WriteLine("Вхідний масив студентів:");
            PrintArray(data);

            Student[] temp = new Student[data.Length];

            Stopwatch sw = Stopwatch.StartNew();
            MergeSort(data, 0, data.Length - 1, temp);
            sw.Stop();

            Console.WriteLine("\nМасив після сортування MergeSort (за зростанням оцінки):");
            PrintArray(data);

            Console.WriteLine("\n⏱️ Час сортування: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");

            // Перевірка стабільності в коді
            int ivanIdx = -1, mariaIdx = -1;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Name == "Іван") ivanIdx = i;
                if (data[i].Name == "Марія") mariaIdx = i;
            }

            if (ivanIdx < mariaIdx)
            {
                Console.WriteLine("\n✅ Результат: Сортування СТАБІЛЬНЕ. Іван залишився перед Марією.");
            }
            else
            {
                Console.WriteLine("\n❌ Результат: Сортування НЕСТАБІЛЬНЕ.");
            }
        }
    }
}
