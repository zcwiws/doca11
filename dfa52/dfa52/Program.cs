using System;
using System.Collections.Generic;

namespace AVLTreeProject
{
    public class AVLNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public int Height { get; set; }
        public AVLNode<TKey, TValue> Left { get; set; }
        public AVLNode<TKey, TValue> Right { get; set; }

        public AVLNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Height = 1;
        }
    }

    public class AVLTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private AVLNode<TKey, TValue> root;

        #region Основные методы AVL-дерева (описание как в предыдущих ответах)

        public void Insert(TKey key, TValue value)
        {
            root = Insert(root, key, value);
        }

        private AVLNode<TKey, TValue> Insert(AVLNode<TKey, TValue> node, TKey key, TValue value)
        {
            if (node == null) return new AVLNode<TKey, TValue>(key, value);

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0) node.Left = Insert(node.Left, key, value);
            else if (cmp > 0) node.Right = Insert(node.Right, key, value);
            else node.Value = value;

            return Balance(node);
        }

        public void Delete(TKey key)
        {
            root = Delete(root, key);
        }

        private AVLNode<TKey, TValue> Delete(AVLNode<TKey, TValue> node, TKey key)
        {
            if (node == null) return null;

            int cmp = key.CompareTo(node.Key);
            if (cmp < 0) node.Left = Delete(node.Left, key);
            else if (cmp > 0) node.Right = Delete(node.Right, key);
            else
            {
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                var temp = GetMinNode(node.Right);
                node.Key = temp.Key;
                node.Value = temp.Value;
                node.Right = Delete(node.Right, temp.Key);
            }
            return Balance(node);
        }

        public AVLNode<TKey, TValue> Search(TKey key)
        {
            var current = root;
            while (current != null)
            {
                int cmp = key.CompareTo(current.Key);
                if (cmp == 0) return current;
                current = cmp < 0 ? current.Left : current.Right;
            }
            return null;
        }

        public TKey GetMin() => GetMinNode(root).Key;
        private AVLNode<TKey, TValue> GetMinNode(AVLNode<TKey, TValue> node)
        {
            while (node.Left != null) node = node.Left;
            return node;
        }

        public TKey GetMax()
        {
            var node = root;
            while (node.Right != null) node = node.Right;
            return node.Key;
        }

        public int Count() => CountNodes(root);
        private int CountNodes(AVLNode<TKey, TValue> node) => node == null ? 0 : 1 + CountNodes(node.Left) + CountNodes(node.Right);

        public int GetHeight() => GetHeight(root);
        private int GetHeight(AVLNode<TKey, TValue> node) => node?.Height ?? 0;

        private AVLNode<TKey, TValue> Balance(AVLNode<TKey, TValue> node)
        {
            UpdateHeight(node);
            if (GetBalance(node) > 1)
            {
                if (GetBalance(node.Left) < 0) node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }
            if (GetBalance(node) < -1)
            {
                if (GetBalance(node.Right) > 0) node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }
            return node;
        }

        private int GetBalance(AVLNode<TKey, TValue> node) => GetHeight(node.Left) - GetHeight(node.Right);
        private void UpdateHeight(AVLNode<TKey, TValue> node) => node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        private AVLNode<TKey, TValue> RotateLeft(AVLNode<TKey, TValue> x)
        {
            var y = x.Right;
            x.Right = y.Left;
            y.Left = x;
            UpdateHeight(x);
            UpdateHeight(y);
            return y;
        }

        private AVLNode<TKey, TValue> RotateRight(AVLNode<TKey, TValue> y)
        {
            var x = y.Left;
            y.Left = x.Right;
            x.Right = y;
            UpdateHeight(y);
            UpdateHeight(x);
            return x;
        }

        public string TreeToString() => TreeToString(root);
        private string TreeToString(AVLNode<TKey, TValue> node)
        {
            if (node == null) return "";
            return $"({TreeToString(node.Left)} {node.Key}:{node.Value} {TreeToString(node.Right)})";
        }

        #endregion
    }

    class Program
    {
        static void Main()
        {
            var avlTree = new AVLTree<int, string>();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nВыберите операцию:");
                Console.WriteLine("1. Добавить узел");
                Console.WriteLine("2. Удалить узел");
                Console.WriteLine("3. Найти узел");
                Console.WriteLine("4. Найти минимум и максимум");
                Console.WriteLine("5. Получить высоту дерева");
                Console.WriteLine("6. Подсчитать количество узлов");
                Console.WriteLine("7. Проверить сбалансированность");
                Console.WriteLine("8. Вывести дерево");
                Console.WriteLine("0. Выйти");

                Console.Write("\nВведите номер задачи: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите ключ: ");
                        int key = int.Parse(Console.ReadLine());
                        Console.Write("Введите значение: ");
                        string value = Console.ReadLine();
                        avlTree.Insert(key, value);
                        break;

                    case "2":
                        Console.Write("Введите ключ для удаления: ");
                        key = int.Parse(Console.ReadLine());
                        avlTree.Delete(key);
                        Console.WriteLine("Узел удалён.");
                        break;

                    case "3":
                        Console.Write("Введите ключ для поиска: ");
                        key = int.Parse(Console.ReadLine());
                        var node = avlTree.Search(key);
                        Console.WriteLine(node != null ? $"Найден: {node.Key} -> {node.Value}" : "Узел не найден.");
                        break;

                    case "4":
                        Console.WriteLine($"Минимум: {avlTree.GetMin()}");
                        Console.WriteLine($"Максимум: {avlTree.GetMax()}");
                        break;

                    case "5":
                        Console.WriteLine($"Высота дерева: {avlTree.GetHeight()}");
                        break;

                    case "6":
                        Console.WriteLine($"Количество узлов: {avlTree.Count()}");
                        break;

                    case "7":
                        Console.WriteLine(avlTree.GetHeight() > 1 ? "Дерево сбалансировано." : "Дерево несбалансировано.");
                        break;

                    case "8":
                        Console.WriteLine($"Дерево: {avlTree.TreeToString()}");
                        break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}