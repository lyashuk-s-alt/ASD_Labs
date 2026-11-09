using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab12
{
    // Реализация Deque на кольцевом массиве с гарантированным O(1) для всех операций
    public class MyDeque<T>
    {
        private T[] _array;
        private int _head;
        private int _tail;
        private int _count;

        public int Count => _count;
        public bool IsEmpty => _count == 0;

        public MyDeque(int initialCapacity = 4)
        {
            _array = new T[initialCapacity];
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        // Вспомогательный метод для автоматического увеличения емкости массива
        private void Resize()
        {
            int newCapacity = _array.Length * 2;
            T[] newArray = new T[newCapacity];

            // Корректно копируем элементы по кругу начиная от головы (_head)
            for (int i = 0; i < _count; i++)
            {
                newArray[i] = _array[(_head + i) % _array.Length];
            }

            _array = newArray;
            _head = 0;
            _tail = _count;
        }

        // Добавление в начало
        public void AddFirst(T item)
        {
            if (_count == _array.Length)
            {
                Resize();
            }

            // Сдвигаем указатель головы влево циклически
            _head = (_head - 1 + _array.Length) % _array.Length;
            _array[_head] = item;
            _count++;
        }

        // Добавление в конец
        public void AddLast(T item)
        {
            if (_count == _array.Length)
            {
                Resize();
            }

            // Записываем в текущую позицию хвоста и сдвигаем его вправо циклически
            _array[_tail] = item;
            _tail = (_tail + 1) % _array.Length;
            _count++;
        }

        // Удаление с начала
        public T RemoveFirst()
        {
            if (IsEmpty) 
                throw new InvalidOperationException("Deque порожній!");

            T val = _array[_head];
            _array[_head] = default(T); // Очистка ссылки для сборщика мусора
            
            _head = (_head + 1) % _array.Length;
            _count--;
            
            return val;
        }

        // Удаление с конца
        public T RemoveLast()
        {
            if (IsEmpty) 
                throw new InvalidOperationException("Deque порожній!");

            // Сдвигаем указатель хвоста влево циклически
            _tail = (_tail - 1 + _array.Length) % _array.Length;
            
            T val = _array[_tail];
            _array[_tail] = default(T); // Очистка ссылки
            _count--;
            
            return val;
        }
    }

    class Program
    {
        // Алгоритм проверки палиндрома с помощью Deque
        static bool IsPalindrome(string text)
        {
            MyDeque<char> deque = new MyDeque<char>();

            // Очищаем текст от пробелов, знаков препинания и приводим к нижнему регистру
            foreach (char c in text.ToLower())
            {
                if (char.IsLetterOrDigit(c))
                {
                    deque.AddLast(c);
                }
            }

            // Сравниваем символы с обоих концов очереди
            while (deque.Count > 1)
            {
                char first = deque.RemoveFirst();
                char last = deque.RemoveLast();
                
                if (first != last) 
                    return false;
            }

            return true;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №12: DEQUE НА МАСИВІ ТА ПАЛІНДРОМИ    ");
            Console.WriteLine("=========================================================================");

            string[] samples = {
                "А роза упала на лапу Азора",
                "Я несу гусеня",
                "Козак з казок",
                "Алгоритми та структури даних",
                "12321",
                "123321"
            };

            Stopwatch sw = Stopwatch.StartNew();

            foreach (string s in samples)
            {
                bool result = IsPalindrome(s);
                string status = result ? "✅ ПАЛІНДРОМ" : "❌ НЕ паліндром";
                Console.WriteLine($"Рядок: \"{s}\" -> {status}");
            }

            sw.Stop();
            Console.WriteLine($"\n⏱️ Час перевірки: {sw.Elapsed.TotalMilliseconds:F4} ms");
        }
    }
}
