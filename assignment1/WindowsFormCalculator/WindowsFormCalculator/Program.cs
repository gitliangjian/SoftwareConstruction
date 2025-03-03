using System;
using System.Windows.Forms;
using WindowsFormCalculator;

namespace WindowsFormsCalculator
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CalculatorForm()); // 这里指定启动窗体
        }
    }
}