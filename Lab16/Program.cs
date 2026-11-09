using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab16
{
    class Program
    {
        // 1. Наївна рекурсія Фібоначчі - О(2^n)
        static long NaiveFibonacci(int n, ref long callCount)
        {
            callCount++;
            if (n <= 1) return n;
            return NaiveFibonacci(n - 1, ref callCount) + NaiveFibonacci(n - 2, ref callCount);
        }

        // 2. Оптимізований Фібоначчі з мемоізацією - О(n)
        static long MemoizedFibonacci(int n, Dictionary<int, long> memo, ref long callCount)
        {
            callCount++;
            if (n <= 1) return n;
            if (memo.ContainsKey(n)) return memo[n];

            long result = MemoizedFibonacci(n - 1, memo, ref callCount) + MemoizedFibonacci(n - 2, memo, ref callCount);
            memo[n] = result;
            return result;
        }

        // 3. Демонстрація глибини рекурсії
        static void DemonstrateRecursionDepth(int currentDepth, int targetDepth)
        {
            if (currentDepth >= targetDepth)
            {
                Console.WriteLine("  Досягнуто безпечну максимальну глибину стеку: " + targetDepth);
                return;
            }
            DemonstrateRecursionDepth(currentDepth + 1, targetDepth);
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Рекурсивний алгоритм Евкліда для НСД (GCD)
        static long GetGCD(long a, long b, ref long gcdCalls)
        {
            gcdCalls++;
            if (b == 0) return a; // Базовий випадок
            return GetGCD(b, a % b, ref gcdCalls); // Крок рекурсії
        }

        // ТОЧКА ВХОДУ (ВИПРАВЛЕНО: додано масив string[] замість одинарного string)
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБОРАТОРНА РОБОТА №16: РЕКУРСІЯ ТА АЛГОРИТМ ЕВКЛІДА     ");
            Console.WriteLine("=========================================================================");

            int n = 35;
            Console.WriteLine("Обчислюємо " + n + "-е число Фібоначчі:\n");

            // Тест 1: Наївна рекурсія
            long calls1 = 0;
            Stopwatch sw = Stopwatch.StartNew();
            long fib1 = NaiveFibonacci(n, ref calls1);
            sw.Stop();
            Console.WriteLine("1. Наївна рекурсія:");
            Console.WriteLine("   Результат: " + fib1 + " | Викликів функції: " + calls1 + " | Час: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");

            // Тест 2: Мемоізація (Кеш)
            long calls2 = 0;
            var memo = new Dictionary<int, long>();
            sw.Restart();
            long fib2 = MemoizedFibonacci(n, memo, ref calls2);
            sw.Stop();
            Console.WriteLine("\n2. Мемоізація (Кеш):");
            Console.WriteLine("   Результат: " + fib2 + " | Викликів функції: " + calls2 + " | Час: " + sw.Elapsed.TotalMilliseconds.ToString("F4") + " ms");

            // Тест 3: Глибина рекурсії
            Console.WriteLine("\n3. Перевірка глибини рекурсивних викликів:");
            DemonstrateRecursionDepth(1, 1000);

            // ТЕСТ ІНДИВІДУАЛЬНОГО ЗАВДАННЯ (Варіант 2)
            Console.WriteLine("\n-------------------------------------------------------------------------");
            Console.WriteLine("Перевірка Індивідуального завдання (Варіант 2 - Алгоритм Евкліда):");
            long num1 = 1071;
            long num2 = 462;
            long gcdCalls = 0;

            long gcdResult = GetGCD(num1, num2, ref gcdCalls);
            Console.WriteLine("  НСД(" + num1 + ", " + num2 + ") = " + gcdResult);
            Console.WriteLine("  Kількість рекурсивних викликів для НСД: " + gcdCalls);
        }
    }
}
