using System;

namespace Program
{
    class IntArray
    {
        private static int Getmax(int[] array )
        {
            int max = 0;
            foreach(int i in array )
            {
                if ( i > max ) max = i;
            }
            return max;
        }

        private static int Getmin(int[] array )
        {
            int min = array[0];
            foreach(int i in array )
            {
                if( i < min ) min = i;
            }
            return min;
        }

        private static double Getaverage(int[] array )
        {
            double avg = 0;
            int sum = 0;
            foreach(int i in array)
            {
                sum += i;
            }
            avg = (double)sum / array.Length;
            return avg;
        }
        static void Main(string[] args)
        {
            int[] numbers = { 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            Console.WriteLine(Getmax(numbers));
            Console.WriteLine(Getmin(numbers));
            Console.WriteLine(Getaverage(numbers));

        }
    }
}