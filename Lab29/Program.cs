using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab29
{
    class Program
    {
        // Базовий метод BFS для знаходження найкоротшого шляху між двома точками
        static List<int> ShortestPathBFS(Dictionary<int, List<int>> graph, int start, int target)
        {
            Queue<int> queue = new Queue<int>();
            HashSet<int> visited = new HashSet<int>();
            Dictionary<int, int> parent = new Dictionary<int, int>();

            queue.Enqueue(start);
            visited.Add(start);

            bool found = false;

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                if (current == target)
                {
                    found = true;
                    break;
                }

                foreach (int neighbor in graph[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        parent[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            if (!found) return null;

            List<int> path = new List<int>();
            int curr = target;
            while (curr != start)
            {
                path.Add(curr);
                curr = parent[curr];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Обчислення відстаней від стартової вершини до ВСІХ інших
        static Dictionary<int, int> ComputeAllDistancesBFS(Dictionary<int, List<int>> graph, int start)
        {
            Queue<int> queue = new Queue<int>();
            Dictionary<int, int> distances = new Dictionary<int, int>();

            // Заносимо стартову вершину, відстань до самої себе дорівнює 0
            queue.Enqueue(start);
            distances[start] = 0;

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();
                int currentDist = distances[current];

                foreach (int neighbor in graph[current])
                {
                    // Якщо вершина ще не має обчисленої відстані, значить вона зустрічається вперше
                    if (!distances.ContainsKey(neighbor))
                    {
                        distances[neighbor] = currentDist + 1;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return distances;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №29: ПОШУК У ШИРИНУ ТА ВІДСТАНІ ДО ВСІХ ВЕРШИН (V2)  ");
            Console.WriteLine("=========================================================================");

            // Створюємо граф комп'ютерної мережі зі скриншоту
            Dictionary<int, List<int>> network = new Dictionary<int, List<int>>();
            network[0] = new List<int> { 1, 2 };
            network[1] = new List<int> { 0, 3, 4 };
            network[2] = new List<int> { 0, 5 };
            network[3] = new List<int> { 1, 6 };
            network[4] = new List<int> { 1, 5 };
            network[5] = new List<int> { 2, 4, 6 };
            network[6] = new List<int> { 3, 5 };

            int startNode = 0;
            int targetNode = 6;

            Console.WriteLine("Шукаємо найкоротший шлях від Вузла [" + startNode + "] до Вузла [" + targetNode + "]:");
            List<int> path = ShortestPathBFS(network, startNode, targetNode);

            if (path != null)
            {
                Console.WriteLine("  Шлях знайдено за " + (path.Count - 1) + " кроків.");
                Console.WriteLine("  Траєкторія: " + string.Join(" -> ", path));
            }
            else
            {
                Console.WriteLine("  Шляху між вузлами не існує!");
            }

            // ТЕСТ ІНДИВІДУАЛЬНОГО ЗАВДАННЯ (Варіант 2)
            Console.WriteLine("\n-------------------------------------------------------------------------");
            Console.WriteLine("Обчислення найкоротших відстаней (кількості ребер) від Вузла [" + startNode + "] до ВСІХ інших:");
            
            Dictionary<int, int> allDistances = ComputeAllDistancesBFS(network, startNode);

            foreach (var kvp in allDistances)
            {
                Console.WriteLine("  Відстань від Вузла [" + startNode + "] до Вузла [" + kvp.Key + "] = " + kvp.Value + " ребер");
            }
        }
    }
}
