using System;
using System.Diagnostics;

namespace Rozumatic_ASD_Lab25
{
    public class TreeNode
    {
        public int Value;
        public TreeNode Left;
        public TreeNode Right;

        public TreeNode(int val) { Value = val; }
    }

    public class BinarySearchTree
    {
        public TreeNode Root;

        // 1. Вставка нового значення в BST - O(log n)
        public void Insert(int val)
        {
            Root = InsertRec(Root, val);
        }

        private TreeNode InsertRec(TreeNode node, int val)
        {
            if (node == null) return new TreeNode(val);

            if (val < node.Value)
                node.Left = InsertRec(node.Left, val);
            else if (val > node.Value)
                node.Right = InsertRec(node.Right, val);

            return node;
        }

        // 2. Пошук значення в BST - O(log n)
        public bool Search(int val)
        {
            return SearchRec(Root, val);
        }

        private bool SearchRec(TreeNode node, int val)
        {
            if (node == null) return false;
            if (node.Value == val) return true;

            if (val < node.Value)
                return SearchRec(node.Left, val);
            else
                return SearchRec(node.Right, val);
        }

        // 3. Видалення вузла (обробка 3 випадків)
        public void Delete(int val)
        {
            Root = DeleteRec(Root, val);
        }

        private TreeNode DeleteRec(TreeNode node, int val)
        {
            if (node == null) return null;

            if (val < node.Value)
                node.Left = DeleteRec(node.Left, val);
            else if (val > node.Value)
                node.Right = DeleteRec(node.Right, val);
            else
            {
                // Випадок 1 та 2: вузол з 0 або 1 дитиною
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                // Випадок 3: вузол з 2 нащадками
                // Знаходимо найменший елемент у правому піддереві (In-order Successor)
                node.Value = MinValue(node.Right);

                // Видаляємо дублікат наступника з правого піддерева
                node.Right = DeleteRec(node.Right, node.Value);
            }

            return node;
        }

        private int MinValue(TreeNode node)
        {
            int minv = node.Value;
            while (node.Left != null)
            {
                minv = node.Left.Value;
                node = node.Left;
            }
            return minv;
        }

        // Симетричний обхід для виведення відсортованого списку
        public void PrintInOrder(TreeNode node)
        {
            if (node != null)
            {
                PrintInOrder(node.Left);
                Console.Write(node.Value + " ");
                PrintInOrder(node.Right);
            }
        }

        // ІНДИВІДУАЛЬНЕ ЗАВДАННЯ (Варіант 2): Перевірка дерева на валідність BST
        public bool IsValidBST()
        {
            // Використовуємо long.MinValue та long.MaxValue, щоб уникнути помилок границь з int
            return Validate(Root, long.MinValue, long.MaxValue);
        }

        private bool Validate(TreeNode node, long min, long max)
        {
            if (node == null) return true;

            // Якщо значення вузла виходить за дозволені математичні межі піддерева — це не BST!
            if (node.Value <= min || node.Value >= max) return false;

            // Для лівого піддерева максимум оновлюється значенням поточного вузла
            // Для правого піддерева мінімум оновлюється значенням поточного вузла
            return Validate(node.Left, min, node.Value) && Validate(node.Right, node.Value, max);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №25: БІНАРНЕ ДЕРЕВО ПОШУКУ ТА ВАЛІДАЦІЯ (VAR 2)      ");
            Console.WriteLine("=========================================================================");

            BinarySearchTree bst = new BinarySearchTree();

            // Вставка елементів
            int[] values = { 50, 30, 70, 20, 40, 60, 80, 65 };
            Console.Write("Вставка значень: [ ");
            foreach (int v in values) Console.Write(v + " ");
            Console.WriteLine("]");

            foreach (int v in values) bst.Insert(v);

            Console.Write("\nВпорядкований In-Order вивід дерева: ");
            bst.PrintInOrder(bst.Root);
            Console.WriteLine();

            // Перевірка індивідуального завдання на валідному дереві
            Console.WriteLine("\n--- Перевірка на валідність дерева (Варіант 2) ---");
            Console.WriteLine("  Чи є поточне дерево валідним BST? Відповідь: " + bst.IsValidBST());

            // Навмисно ламаємо інваріант BST для тестування валідатора
            // Ставимо у ліве піддерево (менше 50) значення 99, що порушує логіку пошуку
            bst.Root.Left.Right.Value = 99; 
            Console.WriteLine("  [Порушено інваріант] Змінено внутрішній вузол на 99.");
            Console.WriteLine("  Чи є дерево валідним BST тепер? Відповідь: " + bst.IsValidBST());
        }
    }
}
