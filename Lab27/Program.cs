using System;

namespace Rozumatic_ASD_Lab27
{
    // ==================== СТРУКТУРА AVL-ДЕРЕВА ====================
    public class AVLNode
    {
        public int Key;
        public int Height;
        public AVLNode Left;
        public AVLNode Right;

        public AVLNode(int key)
        {
            Key = key;
            Height = 1;
        }
    }

    public class AVLTree
    {
        public AVLNode Root;

        // Класичний метод отримання висоти для C# 7.0
        private int GetHeight(AVLNode n)
        {
            if (n == null) return 0;
            return n.Height;
        }

        public int GetTreeHeight()
        {
            return GetHeight(Root);
        }

        private int GetBalance(AVLNode n)
        {
            if (n == null) return 0;
            return GetHeight(n.Left) - GetHeight(n.Right);
        }

        // Малий правий поворот навколо вузла y
        private AVLNode RightRotate(AVLNode y)
        {
            AVLNode x = y.Left;
            AVLNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        // Малий лівий поворот навколо вузла x
        private AVLNode LeftRotate(AVLNode x)
        {
            AVLNode y = x.Right;
            AVLNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        public void Insert(int key)
        {
            Root = InsertRec(Root, key);
        }

        private AVLNode InsertRec(AVLNode node, int key)
        {
            if (node == null) return new AVLNode(key);

            if (key < node.Key)
                node.Left = InsertRec(node.Left, key);
            else if (key > node.Key)
                node.Right = InsertRec(node.Right, key);
            else
                return node; // Дублікати заборонено

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Case 1: Left-Left (Малий правий)
            if (balance > 1 && key < node.Left.Key)
                return RightRotate(node);

            // Case 2: Right-Right (Малий лівий)
            if (balance < -1 && key > node.Right.Key)
                return LeftRotate(node);

            // Case 3: Left-Right (Великий лівий)
            if (balance > 1 && key > node.Left.Key)
                {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // Case 4: Right-Left (Великий правий)
            if (balance < -1 && key < node.Right.Key)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }
    }

    // ==================== СТРУКТУРА ЗВИЧАЙНОГО BST ====================
    public class BSTNode
    {
        public int Key;
        public BSTNode Left;
        public BSTNode Right;
        public BSTNode(int key) { Key = key; }
    }

    public class RegularBST
    {
        public BSTNode Root;

        public void Insert(int key)
        {
            Root = InsertRec(Root, key);
        }

        private BSTNode InsertRec(BSTNode node, int key)
        {
            if (node == null) return new BSTNode(key);
            if (key < node.Key) node.Left = InsertRec(node.Left, key);
            else if (key > node.Key) node.Right = InsertRec(node.Right, key);
            return node;
        }

        public int GetMaxDepth(BSTNode node)
        {
            if (node == null) return 0;
            return 1 + Math.Max(GetMaxDepth(node.Left), GetMaxDepth(node.Right));
        }
    }

    // ==================== ГОЛОВНА ПРОГРАМА ====================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========================================================================");
            Console.WriteLine("  РОЗУМАТИК // ЛАБ. №27: ПОРІВНЯННЯ ГЛИБИНИ BST ТА AVL-ДЕРЕВА (VAR 2)   ");
            Console.WriteLine("=========================================================================");

            RegularBST bst = new RegularBST();
            AVLTree avl = new AVLTree();

            int count = 1000;
            Console.WriteLine("Додаємо " + count + " відсортованих чисел (від 1 до " + count + ")...");

            // Генеруємо та додаємо суворо відсортовану послідовність
            for (int i = 1; i <= count; i++)
            {
                bst.Insert(i);
                avl.Insert(i);
            }

            // Отримуємо фінальні показники висоти (глибини)
            int bstDepth = bst.GetMaxDepth(bst.Root);
            int avlDepth = avl.GetTreeHeight();

            Console.WriteLine("\n--- Результати вимірювання глибини дерев ---");
            Console.WriteLine("  Глибина звичайного BST дерева: " + bstDepth);
            Console.WriteLine("  Глибина самобалансованого AVL-дерева: " + avlDepth);

            Console.WriteLine("\nВисновок: AVL-дерево успішно стримало глибину в логарифмічних межах!");
        }
    }
}
