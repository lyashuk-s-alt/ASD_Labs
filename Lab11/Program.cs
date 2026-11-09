using System;
using System.Diagnostics;
using System.Threading;

namespace Rozumatic_ASD_Lab11
{
    public class CircularQueue<T>
    {
        private T[] _buffer;
        private int _head = 0;
        private int _tail = 0;
        private int _count = 0;
        
        // Об'єкт для синхронізації потоків
        private readonly object _lockObj = new object();

        public int Count
        {
            get { lock (_lockObj) return _count; }
        }

        public int Capacity => _buffer.Length;

        public bool IsEmpty
        {
            get { lock (_lockObj) return _count == 0; }
        }

        public bool IsFull
        {
            get { lock (_lockObj) return _count == _buffer.Length; }
        }

        public CircularQueue(int capacity = 5)
        {
            _buffer = new T[capacity];
        }

        // Додавання в чергу (хвіст) - Потокобезпечне
        public void Enqueue(T item)
        {
            lock (_lockObj)
            {
                if (_count == _buffer.Length)
                {
                    throw new InvalidOperationException("Черга повністю заповнена (Queue Overflow)!");
                }

                _buffer[_tail] = item;
                _tail = (_tail + 1) % _buffer.Length;
                _count++;
            }
        }

        // Вилучення з черги (голова) - Потокобезпечне
        public T Dequeue()
        {
            lock (_lockObj)
            {
                if (_count == 0)
                {
                    throw new InvalidOperationException("Черга порожня (Queue Underflow)!");
                }

                T item = _buffer[_head];
                _buffer[_head] = default(T); // ИСПРАВЛЕНО ДЛЯ C# 7.0
                _head = (_head + 1) % _buffer.Length;
                _count--;
                return item;
            }
        }

        // Перегляд першого в черзі без вилучення - Потокобезпечне
        public T Peek()
        {
            lock (_lockObj)
            {
                if (_count == 0) 
                    throw new InvalidOperationException("Черга порожня!");
                
                return _buffer[_head];
            }
        }

        public void Print()
        {
            lock (_lockObj)
            {
                Console.Write($"[Count={_count}/{Capacity}, Head={_head}, Tail={_tail}]: ");
                if (_count == 0)
                {
                    Console.WriteLine("ПОРОЖНЯ");
                    return;
                }

                Console.Write("HEAD -> [ ");
                for (int i = 0; i < _count; i++)
                {
                    int index = (_head + i) % _buffer.Length;
                    Console.Write($"\"{_buffer[index]}\" ");
                }
                Console.WriteLine("] -> TAIL");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №11: КІЛЬЦЕВИЙ БУФЕР (ПОТОКОБЕЗПЕЧНИЙ)  ");
            Console.WriteLine("=========================================================================");

            CircularQueue<string> queue = new CircularQueue<string>(4);

            // Сценарій Producer-Consumer за допомогою потоків
            Thread producer = new Thread(() =>
            {
                string[] docs = { "Doc1.pdf", "Report.docx", "Photo.png", "Invoice.xlsx" };
                foreach (var doc in docs)
                {
                    try
                    {
                        queue.Enqueue(doc);
                        Console.WriteLine($"[Producer] Додано в чергу: {doc}");
                        queue.Print();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Producer Помилка]: {ex.Message}");
                    }
                    Thread.Sleep(200);
                }
            });

            Thread consumer = new Thread(() =>
            {
                Thread.Sleep(300);
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        string item = queue.Dequeue();
                        Console.WriteLine($"[Consumer] Оброблено (Надруковано): {item}");
                        queue.Print();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Consumer Помилка]: {ex.Message}");
                    }
                    Thread.Sleep(400);
                }
            });

            producer.Start();
            consumer.Start();

            producer.Join();
            consumer.Join();

            Console.WriteLine("\n Роботу потокобезпечного кільцевого буфера перевірено успішно!");
        }
    }
}
