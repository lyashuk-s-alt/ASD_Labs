using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab24
{
    public class MyHashTableDoubleHashing<K, V>
    {
        private enum CellState { Empty, Occupied, Deleted }

        private struct Cell
        {
            public K Key;
            public V Value;
            public CellState State;
        }

        private Cell[] _cells;
        private int _count = 0;

        public int Count => _count;
        public int Capacity => _cells.Length;
        public double LoadFactor => (double)_count / _cells.Length;

        public MyHashTableDoubleHashing(int capacity = 8)
        {
            _cells = new Cell[capacity];
            for (int i = 0; i < capacity; i++) 
                _cells[i].State = CellState.Empty;
        }

        // Перша хеш-функція для визначення початкового індексу
        private int Hash1(K key)
        {
            if (key == null) return 0;
            return (key.GetHashCode() & 0x7FFFFFFF) % _cells.Length;
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Друга хеш-функція для обчислення кроку зсуву
        private int Hash2(K key)
        {
            if (key == null) return 1;
            int h = (key.GetHashCode() & 0x7FFFFFFF) % (_cells.Length - 1);
            if (h == 0) h = 1;
            
            // Оскільки місткість таблиці завжди є степенем двійки (8, 16, 32...),
            // крок зсуву обов'язково має бути непарним числом (h | 1), 
            // щоб бути взаємно простим із розміром масиву. Це гарантує обхід усіх комірок.
            return h | 1;
        }

        // Вставка з подвійним хешуванням за O(1) в середньому
        public void Put(K key, V value)
        {
            if (LoadFactor >= 0.65) 
                Resize(_cells.Length * 2);

            int startIdx = Hash1(key);
            int step = Hash2(key);
            int firstDeletedIdx = -1;

            for (int i = 0; i < _cells.Length; i++)
            {
                // Формула подвійного хешування для обчислення індексу колізії
                int idx = (startIdx + i * step) % _cells.Length;

                if (_cells[idx].State == CellState.Occupied)
                {
                    if (Equals(_cells[idx].Key, key))
                    {
                        _cells[idx].Value = value; // Оновлення значення
                        return;
                    }
                }
                else if (_cells[idx].State == CellState.Deleted)
                {
                    if (firstDeletedIdx == -1) 
                        firstDeletedIdx = idx;
                }
                else if (_cells[idx].State == CellState.Empty)
                {
                    // Вставляємо в першу знайдену комірку Deleted (для економії) або в Empty
                    int insertIdx = firstDeletedIdx != -1 ? firstDeletedIdx : idx;
                    _cells[insertIdx].Key = key;
                    _cells[insertIdx].Value = value;
                    _cells[insertIdx].State = CellState.Occupied;
                    _count++;
                    return;
                }
            }
        }

        // Пошук з пропуском комірок Deleted за O(1) в середньому
        public bool TryGet(K key, out V value)
        {
            int startIdx = Hash1(key);
            int step = Hash2(key);

            for (int i = 0; i < _cells.Length; i++)
            {
                int idx = (startIdx + i * step) % _cells.Length;

                if (_cells[idx].State == CellState.Empty)
                {
                    break; // Якщо зустріли Empty - ключа точно немає в таблиці
                }

                if (_cells[idx].State == CellState.Occupied && Equals(_cells[idx].Key, key))
                {
                    value = _cells[idx].Value;
                    return true;
                }
            }

            value = default(K) is V ? (V)(object)default(K) : default(V); // Безпечне обнулення для C# 7.0
            return false;
        }

        // Видалення зі встановленням маркера Deleted (Tombstone)
        public bool Remove(K key)
        {
            int startIdx = Hash1(key);
            int step = Hash2(key);

            for (int i = 0; i < _cells.Length; i++)
            {
                int idx = (startIdx + i * step) % _cells.Length;

                if (_cells[idx].State == CellState.Empty) 
                    break;

                if (_cells[idx].State == CellState.Occupied && Equals(_cells[idx].Key, key))
                {
                    _cells[idx].State = CellState.Deleted; // Перетворюємо на маркер-могильник
                    _cells[idx].Key = default(K);
                    _cells[idx].Value = default(V);
                    _count--;
                    return true;
                }
            }
            return false;
        }

        private void Resize(int newCapacity)
        {
            Console.WriteLine("  [RESIZE] Збільшення місткості до " + newCapacity);
            Cell[] oldCells = _cells;
            _cells = new Cell[newCapacity];
            
            for (int i = 0; i < newCapacity; i++) 
                _cells[i].State = CellState.Empty;
            
            _count = 0;

            foreach (var c in oldCells)
            {
                if (c.State == CellState.Occupied) 
                    Put(c.Key, c.Value);
            }
        }

        // Виправлено під формат C# 7.0 (заміна сучасного switch-виразу на класичний switch)
        public void Print()
        {
            Console.WriteLine("--- Стан комірок (Count=" + _count + "/" + _cells.Length + ") ---");
            for (int i = 0; i < _cells.Length; i++)
            {
                string stateStr = "";
                switch (_cells[i].State)
                {
                    case CellState.Occupied:
                        stateStr = "[" + _cells[i].Key + ": " + _cells[i].Value + "]";
                        break;
                    case CellState.Deleted:
                        stateStr = "☠️ [DELETED]";
                        break;
                    default:
                        stateStr = "⚪ [EMPTY]";
                        break;
                }
                Console.WriteLine("  [" + i + "]: " + stateStr);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №24: ВІДКРИТА АДРЕСАЦІЯ (DOUBLE HASHING) (VAR 2)     ");
            Console.WriteLine("=========================================================================");

            MyHashTableDoubleHashing<int, string> table = new MyHashTableDoubleHashing<int, string>(8);

            // Додаємо елементи, що навмисно створюють конфлікт (колізію) індексів
            table.Put(10, "Зошит");
            table.Put(18, "Ручка (Колізія з 10)");
            table.Put(26, "Олівець (Колізія з 18)");
            table.Put(7, "Книга");

            table.Print();

            Console.WriteLine("\n--- Видаляємо ключ 18 (Комірка стане DELETED) ---");
            table.Remove(18);
            table.Print();

            Console.WriteLine("\n--- Перевірка пошуку ключа 26 (повинен знайти через DEL комірку) ---");
            if (table.TryGet(26, out string val))
            {
                Console.WriteLine("  Успішно знайдено: " + val);
            }

            Console.WriteLine("\n Маркер DELETED зберіг цілісність ланцюжка подвійного зондування!");
        }
    }
}
