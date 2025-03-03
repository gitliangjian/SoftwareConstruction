using System;
using System.Reflection.Metadata;

namespace Program
{
    class Martix
    {
        static bool isMatrix(int[,] matrix)
        {
            //获取长和宽
            int width=matrix.GetLength(1);
            int height=matrix.GetLength(0);
            //Console.WriteLine(width);
            //Console.WriteLine(height);

            //从第二行第二列开始，检查每个元素是否和左上角元素相同
             for(int i=1;i<width;i++)
            {
                for (int j=1;j<height;j++)
                {
                    if (matrix[i,j] != matrix[i - 1,j - 1])
                    {
                        return false;
                    }
                }
            }
           
            return true;
        }

        static void Main(string[] args)
        {
            int[,] array2D = new int[3, 4] {
                {0, 1, 2, 3}, 
                {4, 5, 6, 7}, 
                {8, 9, 10, 11} 
            };
            Console.WriteLine(isMatrix(array2D));
           
        }
    }
}