using System;

namespace RandomGraph
{
    //定义接口用于求面积和判断图形是否合法
    public interface IGraph
    {
        double getArea();
        bool isValid();
    }

    //形状抽象类
    public abstract class Graph : IGraph
    {
        public abstract double getArea();
        public abstract bool isValid();
    }

    //长方形类
    public class Rectangle : Graph
    {
        //属性：长和宽
        public double Length { get; set; }
        public double Width { get; set; }
        //构造函数
        public Rectangle(double length, double width)
        {
            Length = length;
            Width = width;
        }

        //重写getArea、isValid
        public override double getArea()
        {
            return Length * Width;
        }
        public override bool isValid()
        {
            return Length > 0 && Width > 0;
        }
    }

    //正方形类
    public class Square : Rectangle
    {
        //构造函数
        public Square(double sidelength) : base(sidelength, sidelength) { }

    }

    //三角形类
    public class Triangle : Graph
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }

        public Triangle(double sideA, double sideB, double sideC)
        {
            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
        }
        public override double getArea()
        {
            double s = (SideA + SideB + SideC) / 2;  // 半周长
            double area = Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));  // 海伦公式
            return area;
        }
        public override bool isValid()
        {
            return SideA + SideB > SideC && SideA + SideC > SideB && SideB + SideC > SideA;
        }
    }

    public class GenerateGraph
    {
        private static Random _random = new Random();
        public static IGraph generateGraph(out string graphType)
        {
            int shape = _random.Next(1, 4);//用1、2、3创建三种形状
            switch (shape)
            {
                case 1:
                    graphType = "长方形";
                    return new Rectangle(_random.Next(1, 10), _random.Next(1, 10));
                case 2:
                    graphType = "正方形";
                    return new Square(_random.Next(1, 10));
                case 3:
                    graphType = "三角形";
                    return new Triangle(_random.Next(1, 10), _random.Next(1, 10), _random.Next(1, 10));
                default:
                    throw new ArgumentException("Invalid shape type");
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //随机创建十个图形，并求面积和
            double sumArea = 0;
            string graphtype = "";
            for(int i=0;i<10;i++)
            {
                IGraph graph = GenerateGraph.generateGraph(out graphtype);
                if(graph.isValid())
                {
                    double area = graph.getArea();
                    Console.WriteLine($"图形{i+1}：{graphtype}，面积为{area}");
                    sumArea += area;
                }
                else
                {
                    Console.WriteLine($"图形{i + 1}是不合法的");
                }
            }
            Console.WriteLine($"面积和为：{sumArea}");
        }
    }
}