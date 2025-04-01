using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinForm_test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged; // 绑定事件
            dataGridView1.CellValidating += DataGridView1_CellValidating;
        }
        // 公共属性，用于传入和传出数据
        public string OrderNumber
        {
            get => textBox2.Text;
            set => textBox2.Text = value;
        }

        public string CustomerName
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        public string TotalAmount
        {
            get => textBox3.Text;
            set => textBox3.Text = value;
        }

        public DataGridView OrderDetails => dataGridView1;


        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 在表格末尾添加一行
            //dataGridView1.Rows.Add("新数据1", "新数据2", "新数据3", "新数据4");
            // 检查最后一行的四格是否都填满
            if (dataGridView1.Rows.Count > 0)
            {
                DataGridViewRow lastRow = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
                foreach (DataGridViewCell cell in lastRow.Cells)
                {
                    if (cell.Value == null || string.IsNullOrWhiteSpace(cell.Value.ToString()))
                    {
                        MessageBox.Show("请先填满当前行的所有单元格！");
                        return;
                    }
                }
            }

            // 添加新行并进入编辑模式
            int newRowIndex = dataGridView1.Rows.Add("", "", "", "");
            dataGridView1.ReadOnly = false; // 临时允许编辑
            //isAddingRow = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[newRowIndex].Cells[0];
            dataGridView1.BeginEdit(true);
            //UpdateTotalAmount();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataGridView1_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == 1 || e.ColumnIndex == 2))
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string priceStr = row.Cells[1].Value?.ToString() ?? "0";
                string quantityStr = row.Cells[2].Value?.ToString() ?? "0";

                if (double.TryParse(priceStr, out double price) && double.TryParse(quantityStr, out double quantity))
                {
                    double subtotal = price * quantity;
                    row.Cells[3].Value = subtotal.ToString("F2");

                    // 检查当前行是否已完整输入（商品名称、单价、数量都不为空）
                    if (!string.IsNullOrWhiteSpace(row.Cells[0].Value?.ToString()) &&
                        !string.IsNullOrWhiteSpace(priceStr) &&
                        !string.IsNullOrWhiteSpace(quantityStr))
                    {
                        UpdateTotalAmount(); // 仅在整行输入完成时更新总金额
                    }
                }
                else
                {
                    row.Cells[3].Value = "0.00";
                }
            }
        }
        private void DataGridView1_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            // 只验证单价（索引1）和数量（索引2）
            if (e.RowIndex >= 0 && (e.ColumnIndex == 1 || e.ColumnIndex == 2))
            {
                string? input = e.FormattedValue?.ToString();
                if (string.IsNullOrWhiteSpace(input) || !double.TryParse(input, out _))
                {
                    MessageBox.Show("请输入数字！");
                    e.Cancel = true; // 取消编辑，保持焦点在此单元格
                }
            }
        }

        // 计算所有小计的总和并更新 textbox3
        private void UpdateTotalAmount()
        {
            double total = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[3].Value != null && double.TryParse(row.Cells[3].Value.ToString(), out double subtotal))
                {
                    total += subtotal;
                }
            }
            textBox3.Text = total.ToString("F2"); // 显示总金额，保留两位小数
            textBox3.Refresh(); // 强制刷新 textBox3
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 强制刷新控件以获取最新值
            textBox1.Refresh();
            textBox2.Refresh();
            dataGridView1.Refresh();
            // 检查订单编号
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("订单编号不能为空！");
                return;
            }

            // 检查客户姓名
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("客户姓名不能为空！");
                return;
            }

            // 检查 DataGridView1 是否为空
            if (dataGridView1.Rows.Count == 0 || dataGridView1.Rows[0].Cells[0].Value == null)
            {
                MessageBox.Show("订单明细不能为空！");
                return;
            }

            // 检查 DataGridView1 每行数据是否完整（商品名称、单价、数量不为空）
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                for (int i = 0; i < 3; i++) // 检查前三列
                {
                    if (row.Cells[i].Value == null || string.IsNullOrWhiteSpace(row.Cells[i].Value.ToString()))
                    {
                        MessageBox.Show("订单明细中存在未填写的单元格！");
                        return;
                    }
                }
            }
            //UpdateTotalAmount();
            // 如果所有检查通过，提示成功并关闭窗口
            MessageBox.Show("添加成功！");
            this.DialogResult = DialogResult.OK; // 设置返回结果为 OK
            this.Close(); // 关闭 Form2，返回 Form1
        }
    }
}
