using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab28
{
    // 1. Граф на базі Матриці суміжності
    public class GraphMatrix
    {
        private int[,] _matrix;
        private int _numVertices;

        public GraphMatrix(int vertices)
        {
            _numVertices = vertices;
            _matrix = new int[vertices, vertices];
        }

        public void AddEdge(int u, int v, int weight = 1, bool bidirectional = true)
        {
            _matrix[u, v] = weight;
            if (bidirectional) _matrix[v, u] = weight;
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ: Перевірка наявності ребра в матриці за O(1)
        public bool HasEdge(int u, int v)
        {
            if (u < 0 || u >= _numVertices || v < 0 || v >= _numVertices) return false;
            return _matrix[u, v] != 0;
        }

        public void Print()
        {
            Console.WriteLine("--- Матриця суміжності (" + _numVertices + "x" + _numVertices + ") ---");
            Console.Write("    ");
            for (int i = 0; i < _numVertices; i++) Console.Write("[" + i + "] ");
            Console.WriteLine();

            for (int i = 0; i < _numVertices; i++)
            {
                Console.Write("[" + i + "]  ");
                for (int j = 0; j < _numVertices; j++)
                {
                    Console.Write(_matrix[i, j] + "   ");
                }
                Console.WriteLine();
            }
        }
    }

    // 2. Граф на базі Списків суміжності
    public class GraphList
    {
        private Dictionary<int, List<int>> _adj = new Dictionary<int, List<int>>();

        public GraphList(int vertices)
        {
            for (int i = 0; i < vertices; i++) _adj[i] = new List<int>();
        }

        public void AddEdge(int u, int v, bool bidirectional = true)
        {
            _adj[u].Add(v);
            if (bidirectional) _adj[v].Add(u);
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ: Перевірка наявності ребра в списку за O(deg(u))
        public bool HasEdge(int u, int v)
        {
            if (!_adj.ContainsKey(u)) return false;
            return _adj[u].Contains(v); // Пошук у лінійному списку нащадків
        }

        public void Print()
        {
            Console.WriteLine("\n--- Списки суміжності ---");
            foreach (var kvp in _adj)
            {
                Console.Write("  Вершина [" + kvp.Key + "] -> [ ");
                Console.WriteLine(string.Join(", ", kvp.Value) + " ]");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №28: СТРУКТУРИ ПРЕДСТАВЛЕННЯ ГРАФІВ (VAR 2)          ");
            Console.WriteLine("=========================================================================");

            int vertices = 5;
            // 0: Сервер
            // 1: Роутер А, 2: Роутер В
            // 3: Клієнт 1, 4: Клієнт 2

            GraphMatrix gMat = new GraphMatrix(vertices);
            GraphList gList = new GraphList(vertices);

            // Додаємо з'єднання в мережі
            int[][] edges = new int[][] {
                new int[] { 0, 1 },
                new int[] { 0, 2 },
                new int[] { 1, 3 },
                new int[] { 2, 4 },
                new int[] { 1, 2 }
            };

            foreach (var edge in edges)
            {
                gMat.AddEdge(edge[0], edge[1]);
                gList.AddEdge(edge[0], edge[1]);
            }

            // Виведення обох структур
            gMat.Print();
            gList.Print();

            // Перевірка індивідуального завдання (HasEdge)
            Console.WriteLine("\n--- Перевірка наявності ребер за допомогою HasEdge() ---");
            
            // Тест 1: Існуюче ребро
            Console.WriteLine("Перевірка зв'язку між Сервером [0] та Роутером А [1]:");
            Console.WriteLine("  В Матриці: " + gMat.HasEdge(0, 1));
            Console.WriteLine("  В Списку:  " + gList.HasEdge(0, 1));

            // Тест 2: Відсутнє ребро
            Console.WriteLine("Перевірка зв'язку між Роутером А [1] та Клієнтом 2 [4]:");
            Console.WriteLine("  В Матриці: " + gMat.HasEdge(1, 4));
            Console.WriteLine("  В Списку:  " + gList.HasEdge(1, 4));
        }
    }
}
