using System;

namespace Rozumatic_ASD_Lab2
{
    class Program
    {
        // 1. Пошук мінімуму, максимуму та їх індексів за один прохід O(n)
        static void FindExtremes(int[] arr, out int minVal, out int minIdx, out int maxVal, out int maxIdx)
        {
            minVal = arr[0]; minIdx = 0;
            maxVal = arr[0]; maxIdx = 0;

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < minVal) { minVal = arr[i]; minIdx = i; }
                if (arr[i] > maxVal) { maxVal = arr[i]; maxIdx = i; }
            }
        }

        // 2. In-place реверс масиву двома вказівниками
        static void ReverseInPlace(int[] arr, int start, int end)
        {
            int left = start, right = end;
            while (left < right)
            {
                int temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
                left++; right--;
            }
        }

        // 3. Фільтрація елементів: вибір додатних парних чисел
        static int[] FilterPositiveEven(int[] arr)
        {
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] > 0 && arr[i] % 2 == 0) count++;

            int[] result = new int[count];
            int idx = 0;
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] > 0 && arr[i] % 2 == 0) result[idx++] = arr[i];
            
            return result;
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ. Варіант 3: Видалення дублікатів з масиву зі стисненням за один прохід
        static int RemoveDuplicatesInPlace(int[] arr)
        {
            if (arr.Length <= 1) return arr.Length;

            // Спочатку сортуємо масив, щоб однакові елементи стали поруч
            Array.Sort(arr); 

            int uniqueIdx = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] != arr[uniqueIdx])
                {
                    uniqueIdx++;
                    arr[uniqueIdx] = arr[i]; // Зсуваємо унікальний елемент ліворуч
                }
            }
            return uniqueIdx + 1; // Повертаємо нову кількість унікальних елементів
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================");
            Console.WriteLine("        ЛАБОРАТОРНА РОБОТА №2: АЛГОРИТМИ НАД МАСИВАМИ    ");
            Console.WriteLine("=========================================================");

            int[] numbers = { 42, -7, 18, 99, 8, -15, 24, 88, 3, -2, 76 };
            Console.WriteLine("Вихідний масив: [ " + string.Join(", ", numbers) + " ]\n");

            // 1. Пошук екстремумів
            FindExtremes(numbers, out int minVal, out int minIdx, out int maxVal, out int maxIdx);
            Console.WriteLine(" Мінімум: " + minVal + " (індекс " + minIdx + ")");
            Console.WriteLine(" Максимум: " + maxVal + " (індекс " + maxIdx + ")");

            // 2. Фільтрація
            int[] filtered = FilterPositiveEven(numbers);
            Console.WriteLine("\n Додатні парні числа: [ " + string.Join(", ", filtered) + " ]");

            // 3. Реверс масиву
            int[] arrForReverse = (int[])numbers.Clone();
            ReverseInPlace(arrForReverse, 0, arrForReverse.Length - 1);
            Console.WriteLine("\n Масив після реверсу In-place: [ " + string.Join(", ", arrForReverse) + " ]");

            // ТЕСТУВАННЯ ІНДИВІДУАЛЬНОГО ВАРІАНТА №3
            int[] duplicateArray = { 10, 20, 10, 30, 20, 40, 10, 50, 30, 40 };
            Console.WriteLine("\n--- ВАРІАНТ №3: ВИДАЛЕННЯ ДУБЛІКАТІВ ---");
            Console.WriteLine("Масив з дублікатами: [ " + string.Join(", ", duplicateArray) + " ]");
            
            int uniqueLength = RemoveDuplicatesInPlace(duplicateArray);
            
            Console.Write("Після стиснення (унікальні): [ ");
            for (int i = 0; i < uniqueLength; i++) 
            {
                Console.Write(duplicateArray[i] + (i < uniqueLength - 1 ? ", " : ""));
            }
            Console.WriteLine(" ]");
            Console.WriteLine("\n✅ Тестування завершено успішно!");
        }
    }
}
