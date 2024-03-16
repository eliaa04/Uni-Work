using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace familyTree
{
    class Program
    {
        class TreeNode
        {
            public char Name;
            public TreeNode Mother;
            public TreeNode Father;
        }

        static string ToString(TreeNode node)
        {
            if (node.Father != null && node.Mother != null)
                return ToString(node.Father) + ToString(node.Mother) + char.ToUpper(node.Name);
            else
                return $"{char.ToLower(node.Name)}";
        }

        static TreeNode Parse(string text)
        {
            var stack = new Stack<TreeNode>();

            foreach (var c in text)
            {
                var newNode = new TreeNode { Name = char.ToLower(c) };
                if (char.IsUpper(c))
                {
                    newNode.Mother = stack.Pop();
                    newNode.Father = stack.Pop();
                }
                stack.Push(newNode);
            }

            if (stack.Count != 1)
                throw new Exception("Not correct text!");

            return stack.Pop();
        }

        static void Print(List<TreeNode> level, int depth)
        {
            if (depth == 0)
                return;

            var nextLevel = new List<TreeNode>();

            var spaces = Math.Pow(2, depth) - 1; //  <==>   var spaces = (1 << depth) - 1;

            foreach (var n in level)
            {
                for (int s = 0; s < spaces; s++)
                    Console.Write(' ');

                Console.Write(n == null ? ' ' : n.Name);

                for (int s = 0; s < spaces; s++)
                    Console.Write(' ');

                Console.Write(' ');

                nextLevel.Add(n == null ? null : n.Father);
                // и двете са еднакви
                nextLevel.Add(n?.Mother);
            }
            Console.WriteLine();

            Print(nextLevel, depth - 1);
        }

        static int Depth(List<TreeNode> level)
        {
            var nextLevel = new List<TreeNode>();

            foreach (var n in level)
            {
                if (n.Father != null)
                    nextLevel.Add(n.Father);

                if (n.Mother != null)
                    nextLevel.Add(n.Mother);
            }

            if (level.Count == 0)
                return 0;

            return Depth(nextLevel) + 1;

            //  <==>   return nextLevel.Count == 0 ? 0 : Depth(nextLevel) + 1;
        }

        static TreeNode Find(TreeNode node, char name)
        {
            if (node == null)
                return null;

            if (node.Name == name)
                return node;

            //var result = Find(node.Father, name) ?? Find(node.Mother, name); - друг вариант на if-а  <==>

            var result = Find(node.Father, name);
            if (result == null)
                result = Find(node.Mother, name);

            return result;
        }

        static void AddParents(TreeNode root, char child, char father, char mother)
        {
            var node = Find(root, child);
            if (node != null)
            {
                node.Father = new TreeNode { Name = father };
                node.Mother = new TreeNode { Name = mother };
            }
        }
        static void Main(string[] args)
        {
            TreeNode root;

            if (File.Exists("tree"))
            {
                var savedTreeText = File.ReadAllText("tree");
                root = Parse(savedTreeText);
            }
            else
                root = new TreeNode { Name = 'a' };

            while (true)
            {
                Console.Clear();

                var depth = Depth(new List<TreeNode>() { root });
                Print(new List<TreeNode>() { root }, depth);

                Console.WriteLine();
                Console.WriteLine(ToString(root));
                Console.WriteLine();

                Console.WriteLine();
                Console.WriteLine("a - Add parents; f - Find parents; q - Quit");
                var command = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (command)
                {
                    case 'a':
                        Console.Write("Child: ");
                        var child = Console.ReadKey().KeyChar;
                        Console.WriteLine();

                        Console.Write("Father: ");
                        var father = Console.ReadKey().KeyChar;
                        Console.WriteLine();

                        Console.Write("Mother: ");
                        var mother = Console.ReadKey().KeyChar;
                        Console.WriteLine();

                        AddParents(root, child, father, mother);

                        break;
                    case 'f':
                        Console.Write("Name: ");
                        var name = Console.ReadKey().KeyChar;
                        Console.WriteLine();

                        var node = Find(root, name);

                        if (node == null)
                            Console.WriteLine($"{name} not found!");
                        else
                            Console.WriteLine($"Father: {node.Father.Name}, Mother: {node.Mother.Name}");

                        break;
                    case 'q':
                        var treeText = ToString(root);
                        File.WriteAllText("tree", treeText);      //  <==> File.WriteAllText("tree", ToString(root));


                        break;
                    default:
                        break;
                }
            }
        }
    }
}
