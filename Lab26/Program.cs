using System;
using System.Collections.Generic;

namespace Rozumatic_ASD_Lab26
{
    public class TreeNode
    {
        public string Name;
        public TreeNode Left;
        public TreeNode Right;

        public TreeNode(string name) { Name = name; }
    }

    class Program
    {
        // 1. Pre-Order (V-L-R) - Рекурсивний
        static void PreOrder(TreeNode node)
        {
            if (node == null) return;
            Console.Write(node.Name + " ");
            PreOrder(node.Left);
            PreOrder(node.Right);
        }

        // 2. ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Ітеративний In-Order (L-V-R) через явний Stack без рекурсії
        static void IterativeInOrder(TreeNode root)
        {
            if (root == null) return;

            Stack<TreeNode> stack = new Stack<TreeNode>();
            TreeNode current = root;

            while (current != null || stack.Count > 0)
            {
                // Спускаємося до найлівішого нащадка поточного піддерева
                while (current != null)
                {
                    stack.Push(current); // Зберігаємо вузол у стеку перед переходом ліворуч
                    current = current.Left;
                }

                // Поточний вузол став null, значить дійшли до лівого краю.
                // Дістаємо елемент із вершини стеку
                current = stack.Pop();
                Console.Write(current.Name + " "); // Обробили корінь піддерева (Value)

                // Переходимо до правого піддерева
                current = current.Right;
            }
        }

        // 3. Post-Order (L-R-V) - Рекурсивний
        static void PostOrder(TreeNode node)
        {
            if (node == null) return;
            PostOrder(node.Left);
            PostOrder(node.Right);
            Console.Write(node.Name + " ");
        }

        // 4. Level-Order (BFS) через чергу
        static void LevelOrder(TreeNode root)
        {
            if (root == null) return;

            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                TreeNode current = queue.Dequeue();
                Console.Write(current.Name + " ");

                if (current.Left != null) queue.Enqueue(current.Left);
                if (current.Right != null) queue.Enqueue(current.Right);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №26: ІТЕРAТИВНИЙ IN-ORDER ОБХІД ДЕРЕВА (VAR 2)      ");
            Console.WriteLine("=========================================================================");

            // Побудова ієрархічного дерева зі скриншоту:
            //       A
            //      / \
            //     B   C
            //    / \ / \
            //   D  E F  G

            TreeNode root = new TreeNode("A");
            root.Left = new TreeNode("B");
            root.Right = new TreeNode("C");
            
            root.Left.Left = new TreeNode("D");
            root.Left.Right = new TreeNode("E");
            
            root.Right.Left = new TreeNode("F");
            root.Right.Right = new TreeNode("G");

            Console.WriteLine("Дерево побудовано з коренем 'A'. Результати обходів:\n");

            Console.Write("1. Pre-order  (Прямий, V-L-R):       ");
            PreOrder(root);
            Console.WriteLine();

            Console.Write("2. In-order   (Ітеративний, L-V-R):   ");
            IterativeInOrder(root); // Виклик нашого оптимізованого методу Варіанту 2
            Console.WriteLine();

            Console.Write("3. Post-order (Зворотний, L-R-V):    ");
            PostOrder(root);
            Console.WriteLine();

            Console.Write("4. Level-order (Ширина, BFS черга):  ");
            LevelOrder(root);
            Console.WriteLine();

            Console.WriteLine("\n Усі 4 класичні обходи, включаючи ітеративний In-Order, виконано успішно!");
        }
    }
}
