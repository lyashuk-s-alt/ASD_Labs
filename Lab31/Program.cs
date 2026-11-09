using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab31
{
    public class Edge
    {
        public int To;
        public int Weight; // Базова відстань у км
        public int TrafficDelay; // Додаткова затримка через затори у хвилинах

        // Загальний час у дорозі (у хвилинах). Припускаємо середню швидкість 80 км/год,
        // тобто базова кількість хвилин = (Weight * 60) / 80 + TrafficDelay
        public int TotalMinutes => (Weight * 60) / 80 + TrafficDelay;

        public Edge(int to, int weight, int trafficDelay)
        {
            To = to;
            Weight = weight;
            TrafficDelay = trafficDelay;
        }
    }

    class Program
    {
        static void Dijkstra(int start, int target, Dictionary<int, List<Edge>> graph, string[] cityNames)
        {
            int n = cityNames.Length;
            int[] dist = new int[n]; // Масив найменшого часу у хвилинах
            int[] prev = new int[n];
            bool[] visited = new bool[n];

            for (int i = 0; i < n; i++)
            {
                dist[i] = int.MaxValue;
                prev[i] = -1;
            }

            dist[start] = 0;

            for (int count = 0; count < n; count++)
            {
                int u = -1;
                int minDist = int.MaxValue;

                for (int i = 0; i < n; i++)
                {
                    if (!visited[i] && dist[i] < minDist)
                    {
                        minDist = dist[i];
                        u = i;
                    }
                }

                if (u == -1 || dist[u] == int.MaxValue) break;
                visited[u] = true;

                if (u == target) break;

                // Релаксація ребер з урахуванням загального часу у хвилинах (TotalMinutes)
                foreach (var edge in graph[u])
                {
                    int v = edge.To;
                    int cost = edge.TotalMinutes; // Оптимізуємо ХВИЛИНИ

                    if (!visited[v] && dist[u] + cost < dist[v])
                    {
                        dist[v] = dist[u] + cost;
                        prev[v] = u;
                    }
                }
            }

            if (dist[target] == int.MaxValue)
            {
                Console.WriteLine("Маршруту між " + cityNames[start] + " та " + cityNames[target] + " не існує!");
                return;
            }

            Console.WriteLine("Найкоротший час від " + cityNames[start] + " до " + cityNames[target] + " складає: " + dist[target] + " хвилин.");

            // Відновлення маршруту
            List<string> route = new List<string>();
            int curr = target;
            while (curr != -1)
            {
                route.Add(cityNames[curr]);
                curr = prev[curr];
            }
            route.Reverse();

            Console.WriteLine("Оптимальний маршрут GPS (з урахуванням заторів):");
            Console.WriteLine("  " + string.Join(" -> ", route));
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №31: АЛГОРИТМ ДЕЙКСТРИ З УРАХУВАННЯМ ЗАТОРІВ (VAR 2) ");
            Console.WriteLine("=========================================================================");

            string[] cities = { "Житомир", "Київ", "Вінниця", "Рівне", "Львів", "Одеса" };
            // 0: Житомир, 1: Київ, 2: Вінниця, 3: Рівне, 4: Львів, 5: Одеса

            Dictionary<int, List<Edge>> map = new Dictionary<int, List<Edge>>();
            for (int i = 0; i < cities.Length; i++) map[i] = new List<Edge>();

            // Допоміжний метод додавання дороги (параметри: звідки, куди, км, затримка в хвилинах)
            Action<int, int, int, int> addRoad = (u, v, km, delay) =>
            {
                map[u].Add(new Edge(v, km, delay));
                map[v].Add(new Edge(u, km, delay));
            };

            // Моделюємо карту доріг із заторами (наприклад, сильний затор на трасі Київ-Одеса)
            addRoad(0, 1, 140, 15);  // Житомир - Київ (140 км, затор 15 хв)
            addRoad(0, 2, 128, 5);   // Житомир - Вінниця (128 км, затор 5 хв)
            addRoad(0, 3, 188, 10);  // Житомир - Рівне (188 км, затор 10 хв)
            addRoad(3, 4, 210, 0);   // Рівне - Львів (210 км, заторів немає)
            addRoad(2, 5, 425, 20);  // Вінниця - Одеса (425 км, затор 20 хв)
            addRoad(1, 5, 475, 120); // Київ - Одеса (475 км, КРИТИЧНИЙ ЗАТОР 120 хв!)

            Console.WriteLine("Запуск навігатора: Житомир -> Львів...");
            Dijkstra(0, 4, map, cities);

            Console.WriteLine("\nЗапуск навігатора: Житомир -> Одеса...");
            Dijkstra(0, 5, map, cities);
        }
    }
}
