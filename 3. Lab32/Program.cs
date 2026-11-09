using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab32
{
    class Program
    {
        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Задача про розмін монет через ДП з бектрекінгом
        static void SolveCoinChange(int[] coins, int amount)
        {
            // dp[i] буде зберігати мінімальну кількість монет для суми i
            int[] dp = new int[amount + 1];
            
            // coinUsed[i] зберігає номінал останньої монети, взятої для оптимального збору суми i
            int[] coinUsed = new int[amount + 1]; 

            // Ініціалізація таблиці великими значеннями (аналог нескінченності)
            for (int i = 1; i <= amount; i++)
            {
                dp[i] = amount + 1; 
                coinUsed[i] = -1;
            }
            
            // Базовий випадок: для суми 0 потрібно 0 монет
            dp[0] = 0;

            // 1. Заповнення таблиці динамічного програмування
            for (int i = 1; i <= amount; i++)
            {
                for (int j = 0; j < coins.Length; j++)
                {
                    if (coins[j] <= i)
                    {
                        // Перевіряємо, чи отримаємо ми меншу кількість монет, якщо візьмемо монету coins[j]
                        if (dp[i - coins[j]] + 1 < dp[i])
                        {
                            dp[i] = dp[i - coins[j]] + 1;
                            coinUsed[i] = coins[j]; // Запам'ятовуємо монету для бектрекінгу
                        }
                    }
                }
            }

            // Якщо значення не змінилося, значить суму розміняти неможливо
            if (dp[amount] > amount)
            {
                Console.WriteLine("Неможливо розміняти суму " + amount + " грн заданими номіналами монет.");
                return;
            }

            Console.WriteLine("Мінімальна кількість монет для розміну суми " + amount + " грн: " + dp[amount]);

            // 2. Відновлення списку обраних монет (Бектрекінг)
            List<int> resultCoins = new List<int>();
            int remAmount = amount;
            
            while (remAmount > 0)
            {
                resultCoins.Add(coinUsed[remAmount]);
                remAmount -= coinUsed[remAmount]; // Зменшуємо залишок суми
            }

            Console.WriteLine("\nОптимальний набір монет для видачі решти:");
            foreach (int coin in resultCoins)
            {
                Console.Write("[" + coin + " грн] ");
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №32: ДИНАМІЧНЕ ПРОГРАМУВАННЯ (COIN CHANGE) (V2)      ");
            Console.WriteLine("=========================================================================");

            // Доступні номінали монет в обігу
            int[] coins = { 1, 2, 5, 10 };
            int targetAmount = 28; // Сума для розміну

            Console.Write("Доступні номінали монет: ");
            foreach (int c in coins) Console.Write(c + " ");
            Console.WriteLine("\nСума для розміну: " + targetAmount + " грн\n");

            SolveCoinChange(coins, targetAmount);

            Console.WriteLine("\n=========================================================================");
            Console.WriteLine(" 🎉 ВІТАЄМО! ВИ УСПІШНО ОПАНУВАЛИ ВСІ 32 ТЕМИ АЛГОРИТМІВ ТА СТРУКТУР ДАНИХ!");
            Console.WriteLine("=========================================================================");
        }
    }
}
