using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab13
{
    public class PriorityItem<T>
    {
        public T Data { get; set; }
        public int Priority { get; set; } // Чем больше число - тем выше приоритет!

        public PriorityItem(T data, int priority)
        {
            Data = data;
            Priority = priority;
        }
    }

    public class MyPriorityQueue<T>
    {
        private List<PriorityItem<T>> _items = new List<PriorityItem<T>>();

        public int Count => _items.Count;
        public bool IsEmpty => _items.Count == 0;

        // Вставка элемента с упорядочиванием (Insertion Sort logic) - O(n)
        public void Enqueue(T data, int priority)
        {
            PriorityItem<T> newItem = new PriorityItem<T>(data, priority);

            // Ищем правильную позицию для вставки, чтобы список оставался отсортирован по возрастанию
            int insertIndex = 0;
            while (insertIndex < _items.Count && _items[insertIndex].Priority <= priority)
            {
                insertIndex++;
            }

            // Вставляем элемент на его законное место
            _items.Insert(insertIndex, newItem);
        }

        // Извлечение элемента с наивысшим приоритетом - O(1)
        public T Dequeue()
        {
            if (IsEmpty) 
                throw new InvalidOperationException("Черга порожня!");

            // Так как список отсортирован по возрастанию, элемент с максимальным приоритетом всегда в конце
            int lastIndex = _items.Count - 1;
            T result = _items[lastIndex].Data;
            
            // Удаление с конца списка в C# выполняется за O(1)
            _items.RemoveAt(lastIndex);
            
            return result;
        }

        public void Print()
        {
            Console.WriteLine($"[PriorityQueue Count={Count}]:");
            foreach (var item in _items)
            {
                Console.WriteLine($"  • Пріоритет [{item.Priority}] -> {item.Data}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №13: PRIORITY QUEUE (ВСТАВКА З СОРТУВАННЯМ) ");
            Console.WriteLine("=========================================================================");

            MyPriorityQueue<string> hospitalQueue = new MyPriorityQueue<string>();

            // Добавляем пациентов в приемное отделение с разной срочностью (1-10)
            hospitalQueue.Enqueue("Пацієнт А (Плановий огляд)", 1);
            hospitalQueue.Enqueue("Пацієнт Б (Гострий апендицит)", 9);
            hospitalQueue.Enqueue("Пацієнт В (Застуда)", 2);
            hospitalQueue.Enqueue("Пацієнт Г (Інфаркт міокарда)", 10);
            hospitalQueue.Enqueue("Пацієнт Д (Перелом пальця)", 5);

            // Выводим текущую очередь (она уже автоматически отсортирована внутри)
            hospitalQueue.Print();

            Console.WriteLine("\n--- Черговість прийому лікарями за пріоритетом ---");
            int order = 1;
            while (!hospitalQueue.IsEmpty)
            {
                string patient = hospitalQueue.Dequeue();
                Console.WriteLine($"  #{order++} На прийом викликається: {patient}");
            }

            Console.WriteLine("\n Усі пацієнти прийняті в строгому порядку пріоритету!");
        }
    }
}
