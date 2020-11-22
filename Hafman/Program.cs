using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Huffman
{
    class Node
    {
        public int frequency;
        public string data;
        public Node leftChild, rightChild;

        public Node(string data, int frequency)
        {
            this.data = data;
            this.frequency = frequency;
        }

        public Node(Node leftChild, Node rightChild)
        {
            this.leftChild = leftChild;   ////Присваиваем значение аргумента, полю класса
            this.rightChild = rightChild;

            this.data = leftChild.data + ":" + rightChild.data;
            this.frequency = leftChild.frequency + rightChild.frequency;
        }
    }

    class Program
    {
        static void Main()
        {

            //Ввод пользователем данных
            List<string> strArr = new List<string>();
            Console.WriteLine("Введите символы словаря через Enter. Enter в пустой строке - окончание ввода.");
            string inp = "";
            while ((inp = Console.ReadLine()) != "")
                strArr.Add(inp);

            Console.WriteLine("Введите количество нахождений символов в тексте через пробел: ");
            var input = Console.ReadLine().Split();
            var L = new List<int>(input.Select(int.Parse));
            

            Console.WriteLine("Введите строку для декодирования");
            var str = Console.ReadLine();

            IList<Node> list = new List<Node>(); //создание последновательных списков

            int[] array = L.ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                list.Add(new Node(strArr[i], array[i]));
            }

            Stack<Node> stack = SortedStack(list);

            while (stack.Count > 1)
            {
                Node leftChild = stack.Pop(); // Pop извлекает и возвращает первый элемент из списка 
                Node rightChild = stack.Pop();

                Node parentNode = new Node(leftChild, rightChild);

                stack.Push(parentNode);

                stack = SortedStack(stack.ToList<Node>());
            }

            Node parentNode1 = stack.Pop();

            GenerateCode(parentNode1, "");
            DecodeData(parentNode1, parentNode1, 0, str);
        }

        //Сортировка
        public static Stack<Node> SortedStack(IList<Node> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[i].frequency > list[j].frequency)
                    {
                        Node tempNode = list[j];
                        list[j] = list[i];
                        list[i] = tempNode;
                    }
                }
            }

            Stack<Node> stack = new Stack<Node>();

            for (int j = 0; j < list.Count; j++)
                stack.Push(list[j]);    //добавляет элемент в стек на первое место

            return stack;
        }

        //Кодирование
        public static void GenerateCode(Node parentNode, string code)
        {
            if (parentNode != null)
            {
                GenerateCode(parentNode.leftChild, code + "0");

                if (parentNode.leftChild == null && parentNode.rightChild == null)
                    Console.WriteLine(parentNode.data + "{" + code + "}");

                GenerateCode(parentNode.rightChild, code + "1");
            }
        }
        public static void DecodeData(Node parentNode, Node currentNode, int pointer, string input)
        {
            if (input.Length == pointer)
            {
                if (currentNode.leftChild == null && currentNode.rightChild == null)
                {
                    Console.WriteLine(currentNode.data);
                }

                return;
            }
            else
            {
                if (currentNode.leftChild == null && currentNode.rightChild == null)
                {
                    Console.WriteLine(currentNode.data);
                    DecodeData(parentNode, parentNode, pointer, input);
                }
                else
                {
                    if (input.Substring(pointer, 1) == "0")
                    {
                        DecodeData(parentNode, currentNode.leftChild, ++pointer, input);
                    }
                    else
                    {
                        DecodeData(parentNode, currentNode.rightChild, ++pointer, input);
                    }
                }
            }
        }
    }
}