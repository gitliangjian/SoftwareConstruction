using System;

namespace Program
{
    class SieveMethod
    {
        static bool[] SieveGetPrime(int n)
        {
            // 创建一个布尔类型的数组，表示每个数字是否为素数
            bool[] isPrime = new bool[n + 1];

            // 初始时，所有数字都假设为素数
            for (int i = 2; i <= n; i++)
            {
                isPrime[i] = true;
            }

            // 从2开始，逐步筛除合数
            for (int p = 2; p * p <= n; p++)
            {
                // 如果isPrime[p]为true，表示p是素数
                if (isPrime[p])
                {
                    // 将p的倍数标记为合数
                    for (int i = p * p; i <= n; i += p)
                    {
                        isPrime[i] = false;
                    }
                }
            }

            return isPrime;
        }

        static void Main(string[] args)
        {
            bool[] isPrime = SieveGetPrime(200);

            Console.WriteLine("小于等于200的素数有：");
            for (int i = 2; i <= 200; i++)
            {
                if (isPrime[i])
                {
                    Console.Write(i + " ");
                }
            }
        }
    }
}