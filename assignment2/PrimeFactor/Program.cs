using System;

namespace Program
{
    class PrimeFactor
    {
        static int[] GetPrimeFactors(int number, out int count)
        {
            int[] arr_prime = new int[number];//素数因子数组
            count = 0;//素数因子个数
            //先处理偶数
            if (number % 2 == 0)
            {
                arr_prime[count++] = 2;
                while (number % 2 == 0)
                {
                    number = number / 2;
                }
            }
            //再处理奇数因子
            for(int i=3;i*i<=number;i+=2)
            {
                if(number % i == 0)
                {
                    arr_prime[count++] = i;
                    while(number % i == 0)
                    {
                        number = number / i;
                    }
                }
            }
            if(number>1)
            {
                arr_prime[count] = number;
            }
            return arr_prime;
        }
        static void Main(string[] args)
        {   
            //输入一个数字number，用count接收素数因子个数，用PrimeFactors接收素数因子数组
            int number = 126;
            int count ;
            int[]PrimeFactors = GetPrimeFactors(number, out count);
            if (count != 0)
            {
                Console.WriteLine($"{number}的素数因子如下：");
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine(PrimeFactors[i] + " ");
                }
            }
            else Console.WriteLine($"{number}没有素数因子");
            
        }
    }
}
