using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json; // 需要安装 Newtonsoft.Json NuGet 包

namespace WinForm_test
{
    public partial class Form1 : Form
    {
        // 定义订单类存储完整信息
        private class Order
        {
            public string? OrderNumber { get; set; }
            public string? CustomerName { get; set; }
            public double TotalAmount { get; set; }
            public List<OrderItem> Details { get; set; } = new List<OrderItem>();

            //public static implicit operator Order(Order v)
            //{
            //    throw new NotImplementedException();
            //}
        }

        private class OrderItem
        {
            public string? ItemName { get; set; }
            public double Price { get; set; }
            public double Quantity { get; set; }
            public double Subtotal => Price * Quantity;
        }

        private List<Order> orders = new List<Order>(); // 存储所有订单
        private string filePath = Path.Combine(Application.StartupPath, "orders.json"); // 保存文件路径

        public Form1()
        {
            InitializeComponent();

            dataGridView1.CellClick += dataGridView1_CellContentClick; // 添加点击事件

            // 加载已有订单
            LoadOrders();

        }
        private void LoadOrders()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                orders = JsonConvert.DeserializeObject<List<Order>>(json) ?? new List<Order>();

                // 更新 DataGridView1
                dataGridView1.Rows.Clear();
                for (int i = 0; i < orders.Count; i++)
                {
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells[0].Value = i + 1;
                    dataGridView1.Rows[rowIndex].Cells[1].Value = orders[i].OrderNumber;
                    dataGridView1.Rows[rowIndex].Cells[2].Value = orders[i].CustomerName;
                    dataGridView1.Rows[rowIndex].Cells[3].Value = orders[i].TotalAmount.ToString("F2");
                }

                // 默认显示第一条订单的明细
                if (orders.Count > 0)
                {
                    dataGridView1.Rows[0].Selected = true;
                    DisplayOrderDetails(orders[0]);
                }
            }
        }

        private void SaveOrders()
        {
            string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Form2 form2 = new Form2(); // 创建 Form2 实例
            //form2.ShowDialog();
            using (Form2 form2 = new Form2())
            {
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    // 创建新订单
                    Order newOrder = new Order
                    {
                        OrderNumber = form2.OrderNumber,
                        CustomerName = form2.CustomerName,
                        TotalAmount = double.TryParse(form2.TotalAmount, out double t) ? t : 0
                };

                    // 添加订单明细
                    foreach (DataGridViewRow row in form2.OrderDetails.Rows)
                    {
                        if (row.Cells[0].Value == null || string.IsNullOrWhiteSpace(row.Cells[0].Value.ToString()))
                            continue;

                        string? itemName = row.Cells[0].Value.ToString();
                        double price = double.TryParse(row.Cells[1].Value.ToString(), out double p) ? p : 0;
                        double quantity = double.TryParse(row.Cells[2].Value.ToString(), out double q) ? q : 0;

                        newOrder.Details.Add(new OrderItem
                        {
                            ItemName = itemName,
                            Price = price,
                            Quantity = quantity
                        });
                    }

                    // 将订单添加到列表
                    orders.Add(newOrder);

                    // 更新 DataGridView1
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells[0].Value = rowIndex + 1;
                    dataGridView1.Rows[rowIndex].Cells[1].Value = newOrder.OrderNumber;
                    dataGridView1.Rows[rowIndex].Cells[2].Value = newOrder.CustomerName;
                    dataGridView1.Rows[rowIndex].Cells[3].Value = newOrder.TotalAmount.ToString("F2");

                    // 默认选中第一行并显示明细（仅在第一次添加时）
                    if (orders.Count == 1)
                    {
                        dataGridView1.Rows[0].Selected = true;
                        DisplayOrderDetails(orders[0]);
                    }

                    SaveOrders(); // 保存订单到文件
                }
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < orders.Count)
            {
                DisplayOrderDetails(orders[e.RowIndex]);
            }
        }
        private void DisplayOrderDetails(Order order)
        {
            dataGridView2.Rows.Clear(); // 清空现有明细
            if (order != null)
            {
                foreach (OrderItem item in order.Details)
                {
                    int rowIndex = dataGridView2.Rows.Add();
                    dataGridView2.Rows[rowIndex].Cells[0].Value = item.ItemName;
                    dataGridView2.Rows[rowIndex].Cells[1].Value = item.Price.ToString("F2");
                    dataGridView2.Rows[rowIndex].Cells[2].Value = item.Quantity.ToString("F0");
                    dataGridView2.Rows[rowIndex].Cells[3].Value = item.Subtotal.ToString("F2");
                }
            }
        }

        //修改订单
        private void button3_Click(object sender, EventArgs e)
        {
            // 检查是否有选中行
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的订单！");
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow.Cells[1].Value == null || string.IsNullOrWhiteSpace(selectedRow.Cells[1].Value.ToString()))
            {
                MessageBox.Show("选中的订单信息为空，请选择有效订单！");
                return;
            }

            int selectedIndex = selectedRow.Index;
            Order selectedOrder = orders[selectedIndex];

            // 创建 Form2 实例并传入数据
            using (Form2 form2 = new Form2())
            {
                // 将选中订单数据传入 Form2
                form2.OrderNumber = selectedOrder.OrderNumber;
                form2.CustomerName = selectedOrder.CustomerName;
                form2.TotalAmount = selectedOrder.TotalAmount.ToString("F2");
                foreach (var item in selectedOrder.Details)
                {
                    form2.OrderDetails.Rows.Add(item.ItemName, item.Price.ToString("F2"), item.Quantity.ToString("F0"));
                }

                if (form2.ShowDialog() == DialogResult.OK)
                {
                    // 更新订单数据
                    selectedOrder.OrderNumber = form2.OrderNumber;
                    selectedOrder.CustomerName = form2.CustomerName;
                    selectedOrder.TotalAmount = double.TryParse(form2.TotalAmount, out double t) ? t : 0;
                    selectedOrder.Details.Clear();
                    foreach (DataGridViewRow row in form2.OrderDetails.Rows)
                    {
                        if (row.Cells[0].Value == null || string.IsNullOrWhiteSpace(row.Cells[0].Value.ToString()))
                            continue;

                        string? itemName = row.Cells[0].Value.ToString();
                        double price = double.TryParse(row.Cells[1].Value.ToString(), out double p) ? p : 0;
                        double quantity = double.TryParse(row.Cells[2].Value.ToString(), out double q) ? q : 0;

                        selectedOrder.Details.Add(new OrderItem
                        {
                            ItemName = itemName,
                            Price = price,
                            Quantity = quantity
                        });
                    }

                    // 更新 dataGridView1
                    dataGridView1.Rows[selectedIndex].Cells[1].Value = selectedOrder.OrderNumber;
                    dataGridView1.Rows[selectedIndex].Cells[2].Value = selectedOrder.CustomerName;
                    dataGridView1.Rows[selectedIndex].Cells[3].Value = selectedOrder.TotalAmount.ToString("F2");

                    // 更新 dataGridView2
                    DisplayOrderDetails(selectedOrder);

                    // 保存到文件
                    SaveOrders();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 检查是否有选中行
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的订单！");
                return;
            }

            // 判断选中行是否为空
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow.Cells[1].Value == null || string.IsNullOrWhiteSpace(selectedRow.Cells[1].Value.ToString()))
            {
                MessageBox.Show("选中的订单信息为空，请选择有效订单！");
                return;
            }

            // 获取选中行的索引
            int selectedIndex = selectedRow.Index;
            string? orderNumber = selectedRow.Cells[1].Value.ToString();

            // 弹出确认对话框
            DialogResult result = MessageBox.Show($"你确定要删除编号为 {orderNumber} 的订单吗？",
                                                  "确认删除",
                                                  MessageBoxButtons.OKCancel,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                // 从 orders 列表中删除对应订单
                orders.RemoveAt(selectedIndex);

                // 从 dataGridView1 中删除选中行
                dataGridView1.Rows.RemoveAt(selectedIndex);

                // 更新序号，确保不超过当前订单数
                int rowCount = dataGridView1.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = i + 1;
                }

                // 更新 dataGridView2 显示
                if (orders.Count > 0)
                {
                    // 默认选中第一行并显示其明细
                    dataGridView1.Rows[0].Selected = true;
                    DisplayOrderDetails(orders[0]);
                }
                else
                {
                    // 无订单时清空 dataGridView2
                    dataGridView2.Rows.Clear();
                }
            }
        }
    }
}
