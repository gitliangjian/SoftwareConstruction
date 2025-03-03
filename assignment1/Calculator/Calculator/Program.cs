using System;

class Calculator
{
    static void Main()
    {
        double num1 = GetValidNumber("请输入第一个数字：");
        string operation = GetValidOperator();
        double num2 = GetValidNumber("请输入第二个数字：");

        CalculateAndDisplay(num1, num2, operation);
    }

    static double GetValidNumber(string prompt)
    {
        double number;
        Console.Write(prompt);
        while (!double.TryParse(Console.ReadLine(), out number))
        {
            Console.Write("输入无效，请重新输入数字：");
        }
        return number;
    }

    static string GetValidOperator()
    {
        Console.Write("请输入运算符（+、-、*、/）：");
        string input = Console.ReadLine();
        while (!IsValidOperator(input))
        {
            Console.Write("运算符无效，请重新输入（+、-、*、/）：");
            input = Console.ReadLine();
        }
        return input;
    }

    static bool IsValidOperator(string op)
    {
        return op == "+" || op == "-" || op == "*" || op == "/";
    }

    static void CalculateAndDisplay(double num1, double num2, string op)
    {
        try
        {
            double result = op switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                "/" when num2 != 0 => num1 / num2,
                "/" => throw new DivideByZeroException(),
                _ => throw new InvalidOperationException()
            };

            Console.WriteLine($"计算结果：{num1} {op} {num2} = {result}");
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine("错误：除数不能为零！");
        }
    }
}