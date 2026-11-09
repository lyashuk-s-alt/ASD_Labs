using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab30
{
    class Program
    {
        enum Color { White, Gray, Black }

        // 1. Пошук циклів за допомогою триколірного DFS (залишено з базового прикладу)
        static bool HasCycleDFS(int u, Dictionary<int, List<int>> graph, Color[] colors)
        {
            colors[u] = Color.Gray; // У процесі обробки

            foreach (int v in graph[u])
            {
                if (colors[v] == Color.Gray)
                {
                    Console.WriteLine("  ⚠️ Виявлено зворотне ребро {" + u + "} -> {" + v + "}: ГРАФ МІСТИТЬ ЦИКЛ!");
                    return true;
                }

                if (colors[v] == Color.White && HasCycleDFS(v, graph, colors))
                {
                    return true;
                }
            }

            colors[u] = Color.Black; // Повністю оброблено
            return false;
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Топологічне сортування за алгоритмом Кана (Kahn's Algorithm)
        static List<int> TopologicalSortKahn(int vertices, Dictionary<int, List<int>> graph)
        {
            int[] inDegree = new int[vertices];

            // Крок 1: Розрахунок вхідних степеней (in-degree) для кожної вершини
            for (int u = 0; u < vertices; u++)
            {
                foreach (int v in graph[u])
                {
                    inDegree[v]++;
                }
            }

            // Крок 2: Додаємо всі вершини з вхідною степенем 0 у чергу
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < vertices; i++)
            {
                if (inDegree[i] == 0)
                {
                    queue.Enqueue(i);
                }
            }

            List<int> topoOrder = new List<int>();

            // Крок 3: Вилучаємо вершини з черги та зменшуємо степені їхніх сусідів
            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                topoOrder.Add(u);

                foreach (int v in graph[u])
                {
                    inDegree[v]--; // Видаляємо ребро u -> v
                    
                    // Якщо всі передумови виконано, заносимо вершину в чергу
                    if (inDegree[v] == 0)
                    {
                        queue.Enqueue(v);
                    }
                }
            }

            // Крок 4: Перевірка на наявність циклів (якщо оброблено менше вершин, ніж є в графі)
            if (topoOrder.Count != vertices)
            {
                return null; // Цикл унеможливлює топологічне сортування
            }

            return topoOrder;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №30: АЛГОРИТМ КАНА ДЛЯ ТОПОЛОГІЧНОГО СОРТУВАННЯ      ");
            Console.WriteLine("=========================================================================");

            int vertices = 5;
            
            // Граф залежностей між навчальними предметами:
            // 0: Основи програмування -> 1: ООП, 4: Архітектура комп'ютерів
            // 1: ООП -> 2: Алгоритми та структури даних
            // 2: Алгоритми -> 3: Системи штучного інтелекту
            // 3: Системи штучного інтелекту -> немає нащадків
            // 4: Архітектура комп'ютерів -> немає нащадків
            
            Dictionary<int, List<int>> courseDependencies = new Dictionary<int, List<int>>();
            courseDependencies[0] = new List<int> { 1, 4 };
            courseDependencies[1] = new List<int> { 2 };
            courseDependencies[2] = new List<int> { 3 };
            courseDependencies[3] = new List<int>();
            courseDependencies[4] = new List<int>();

            // --- Перевірка на цикли за допомогою триколірного DFS ---
            Color[] colors = new Color[vertices];
            bool hasCycle = false;
            for (int i = 0; i < vertices; i++)
            {
                if (colors[i] == Color.White)
                {
                    if (HasCycleDFS(i, courseDependencies, colors))
                    {
                        hasCycle = true;
                        break;
                    }
                }
            }
            Console.WriteLine("Перевірка на цикли через DFS: " + (hasCycle ? "Знайдено цикл" : "Циклів не виявлено"));

            // --- Топологічне сортування за алгоритмом Кана (Варіант 2) ---
            Console.WriteLine("\n--- Виконання алгоритму Кана (через вхідні степені вершин) ---");
            List<int> result = TopologicalSortKahn(vertices, courseDependencies);

            if (result != null)
            {
                string[] courseNames = { "Основи прогр.", "ООП", "Алгоритми", "Штучний інтелект", "Архітектура комп." };
                Console.WriteLine(" Рекомендований порядок вивчення дисциплін (Topological Order):");
                
                int step = 1;
                foreach (int courseId in result)
                {
                    Console.WriteLine("  Крок " + (step++) + ": [" + courseId + "] " + courseNames[courseId]);
                }
            }
            else
            {
                Console.WriteLine("  Помилка сортування: граф містить циклічні залежності!");
            }
        }
    }
}
