using System;

namespace GaphArea
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

    class Program
    {
        static void Main(string[] args)
        {
            //创建三个不同的图形
            IGraph rectangle = new Rectangle(-1, 5);
            IGraph square = new Square(6);
            IGraph triangle = new Triangle(6, 1, 3);

            //输出图形的相关信息，是否合法以及面积
            GraphInfo(rectangle);
            GraphInfo(square);
            GraphInfo(triangle);
        }

        static void GraphInfo(IGraph graph)
        {
            if(graph.isValid())
            {
                Console.WriteLine($"该图形面积为:{graph.getArea()}");
            }
            else
            {
                Console.WriteLine("该图形不合法");
            }
        }
    }


}