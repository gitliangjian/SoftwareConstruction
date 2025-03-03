using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsCalculator
{
    public partial class CalculatorForm : Form
    {
        public CalculatorForm()
        {
            InitializeComponent();
            InitializeOperatorComboBox();
        }

        private void InitializeOperatorComboBox()
        {
            cmbOperator.Items.AddRange(new object[] { "+", "-", "*", "/" });
            cmbOperator.SelectedIndex = 0;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            double num1 = double.Parse(txtNumber1.Text);
            double num2 = double.Parse(txtNumber2.Text);
            string op = cmbOperator.SelectedItem.ToString();

            try
            {
                double result = Calculate(num1, num2, op);
                lblResult.Text = $"{num1} {op} {num2} = {result:0.####}";
                lblResult.ForeColor = SystemColors.ControlText;
            }
            catch (DivideByZeroException)
            {
                ShowError("除数不能为零！");
            }
        }

        private double Calculate(double num1, double num2, string op)
        {
            switch (op)
            {
                case "+": return num1 + num2;
                case "-": return num1 - num2;
                case "*": return num1 * num2;
                case "/":
                    if (num2 == 0) throw new DivideByZeroException();
                    return num1 / num2;
                default: throw new ArgumentException("无效运算符");
            }
        }

        private bool ValidateInputs()
        {
            if (!double.TryParse(txtNumber1.Text, out _))
            {
                ShowError("第一个数字输入无效");
                return false;
            }

            if (!double.TryParse(txtNumber2.Text, out _))
            {
                ShowError("第二个数字输入无效");
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            lblResult.Text = $"错误：{message}";
            lblResult.ForeColor = System.Drawing.Color.Red;
        }
    }
}