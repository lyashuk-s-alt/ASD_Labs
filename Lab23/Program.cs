using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab23
{
    public class MyHashTableChaining<K, V>
    {
        private class Entry
        {
            public K Key { get; set; }
            public V Value { get; set; }

            public Entry(K key, V value) { Key = key; Value = value; }
        }

        private LinkedList<Entry>[] _buckets;
        private int _count = 0;

        public int Count => _count;
        public int BucketCount => _buckets.Length;
        public double LoadFactor => (double)_count / _buckets.Length;

        public MyHashTableChaining(int initialCapacity = 16)
        {
            _buckets = new LinkedList<Entry>[initialCapacity];
            for (int i = 0; i < initialCapacity; i++)
            {
                _buckets[i] = new LinkedList<Entry>();
            }
        }

        private int GetBucketIndex(K key)
        {
            int h = key == null ? 0 : key.GetHashCode();
            // Маска для уникнення від'ємного індексу та взяття за модулем розміру масиву
            return (h & 0x7FFFFFFF) % _buckets.Length;
        }

        // Вставка або оновлення за O(1) в середньому
        public void Put(K key, V value)
        {
            int index = GetBucketIndex(key);
            var list = _buckets[index];

            foreach (var entry in list)
            {
                if (Equals(entry.Key, key))
                {
                    entry.Value = value; // Оновлення існуючого ключа
                    return;
                }
            }

            // Додаємо новий запис у початок списку
            list.AddFirst(new Entry(key, value));
            _count++;

            // Автоматичний рехешинг при LoadFactor > 0.75
            if (LoadFactor > 0.75)
            {
                Resize(_buckets.Length * 2);
            }
        }

        // Отримання значення за O(1) в середньому
        public bool TryGet(K key, out V value)
        {
            int index = GetBucketIndex(key);
            var list = _buckets[index];

            foreach (var entry in list)
            {
                if (Equals(entry.Key, key))
                {
                    value = entry.Value;
                    return true;
                }
            }

            value = default(V); // Адаптовано під C# 7.0 (без виклику default!)
            return false;
        }

        // Видалення ключа за O(1) в середньому
        public bool Remove(K key)
        {
            int index = GetBucketIndex(key);
            var list = _buckets[index];

            var node = list.First;
            while (node != null)
            {
                if (Equals(node.Value.Key, key))
                {
                    list.Remove(node);
                    _count--;
                    return true;
                }
                node = node.Next;
            }
            return false;
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Підрахунок максимальної довжини ланцюжка
        public int GetMaxChainLength()
        {
            int maxLength = 0;
            for (int i = 0; i < _buckets.Length; i++)
            {
                if (_buckets[i].Count > maxLength)
                {
                    maxLength = _buckets[i].Count;
                }
            }
            return maxLength;
        }

        private void Resize(int newCapacity)
        {
            Console.WriteLine("  [REHASH] Збільшення бакетів з " + _buckets.Length + " до " + newCapacity);
            var oldBuckets = _buckets;
            
            _buckets = new LinkedList<Entry>[newCapacity];
            for (int i = 0; i < newCapacity; i++)
            {
                _buckets[i] = new LinkedList<Entry>();
            }

            _count = 0;

            foreach (var list in oldBuckets)
            {
                foreach (var entry in list)
                {
                    Put(entry.Key, entry.Value);
                }
            }
        }

        public void PrintStats()
        {
            Console.WriteLine("[Таблиця]: Елементів = " + _count + ", Бакетів = " + _buckets.Length + 
                              ", LoadFactor = " + LoadFactor.ToString("F2") + 
                              ", Макс. ланцюжок = " + GetMaxChainLength());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №23: ХЕШ-ТАБЛИЦЯ З МЕТОДОМ ЛАНЦЮЖКІВ (ВАРАНТ 2)      ");
            Console.WriteLine("=========================================================================");

            // Ініціалізуємо невелику таблицю для наочності рехешингу (Capacity = 4)
            MyHashTableChaining<string, string> users = new MyHashTableChaining<string, string>(4);

            Console.WriteLine("--- 1. Додавання записів (з відстеженням рехешингу та збалансованості) ---");
            users.Put("admin", "Адміністратор Сервера");
            users.Put("student1", "Олександр Петренко");
            users.Put("student2", "Ірина Ковальчук");
            users.PrintStats(); // Перевірка до рехешингу
            
            users.Put("teacher", "Микола Михайлович"); // Цей елемент викличе перевищення LoadFactor > 0.75
            users.Put("guest", "Гість Системи");
            users.PrintStats(); // Перевірка після рехешингу

            Console.WriteLine("\n--- 2. Пошук за ключем ---");
            if (users.TryGet("student2", out string name))
            {
                Console.WriteLine("  Знайдено за ключем 'student2': " + name);
            }

            Console.WriteLine("\n--- 3. Видалення ключа ---");
            bool removed = users.Remove("guest");
            Console.WriteLine("  Видалено 'guest': " + removed);
            users.PrintStats();

            Console.WriteLine("\n Роботу власної хеш-таблиці з підрахунком макс. ланцюжка перевірено!");
        }
    }
}
