using System;

namespace GenericList
{
    //泛型链表类和结点
    //链表结点
    public class Node<T>
    {
        public Node<T> Next { get; set; }
        public T Data { get; set; }
        public Node(T data)
        {
            Next = null;
            Data = data;
        }
    }

    public class GenericList<T>
    {
        private Node<T> head;
        private Node<T> tail;

        public GenericList(Node<T> head, Node<T> tail)
        {
            this.head = head;
            this.tail = tail;
        }
        public GenericList()
        {
            tail = head = null;
        }
        public Node<T> Head
        {
            get => head;
        }
        public void Add(T t)
        {
            Node<T> n = new Node<T>(t);
            if(tail ==null)
            {
                head = tail = n;
            }
            else
            {
                tail.Next = n;
                tail = n;
            }
        }
        public void ForEach(Action<T> action)
        {
            Node<T> current = head;
            while (current != null)
            {
                action(current.Data);
                current = current.Next;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GenericList<int> list = new GenericList<int>();
            list.Add(1);
            list.Add(5);
            list.Add(3);
            list.Add(9);
            list.Add(2);

            // 打印链表元素
            Console.WriteLine("Elements:");
            list.ForEach(x => Console.WriteLine(x));

            // 求最大值
            int max = int.MinValue;
            list.ForEach(x => max = x > max ? x : max);
            Console.WriteLine($"Max: {max}");

            // 求最小值
            int min = int.MaxValue;
            list.ForEach(x => min = x < min ? x : min);
            Console.WriteLine($"Min: {min}");

            // 求和
            int sum = 0;
            list.ForEach(x => sum += x);
            Console.WriteLine($"Sum: {sum}");
        }
    }
}
