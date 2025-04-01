using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json; // ��Ҫ��װ Newtonsoft.Json NuGet ��

namespace WinForm_test
{
    public partial class Form1 : Form
    {
        // ���嶩����洢������Ϣ
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

        private List<Order> orders = new List<Order>(); // �洢���ж���
        private string filePath = Path.Combine(Application.StartupPath, "orders.json"); // �����ļ�·��

        public Form1()
        {
            InitializeComponent();

            dataGridView1.CellClick += dataGridView1_CellContentClick; // ��ӵ���¼�

            // �������ж���
            LoadOrders();

        }
        private void LoadOrders()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                orders = JsonConvert.DeserializeObject<List<Order>>(json) ?? new List<Order>();

                // ���� DataGridView1
                dataGridView1.Rows.Clear();
                for (int i = 0; i < orders.Count; i++)
                {
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells[0].Value = i + 1;
                    dataGridView1.Rows[rowIndex].Cells[1].Value = orders[i].OrderNumber;
                    dataGridView1.Rows[rowIndex].Cells[2].Value = orders[i].CustomerName;
                    dataGridView1.Rows[rowIndex].Cells[3].Value = orders[i].TotalAmount.ToString("F2");
                }

                // Ĭ����ʾ��һ����������ϸ
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
            //Form2 form2 = new Form2(); // ���� Form2 ʵ��
            //form2.ShowDialog();
            using (Form2 form2 = new Form2())
            {
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    // �����¶���
                    Order newOrder = new Order
                    {
                        OrderNumber = form2.OrderNumber,
                        CustomerName = form2.CustomerName,
                        TotalAmount = double.TryParse(form2.TotalAmount, out double t) ? t : 0
                };

                    // ��Ӷ�����ϸ
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

                    // ��������ӵ��б�
                    orders.Add(newOrder);

                    // ���� DataGridView1
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells[0].Value = rowIndex + 1;
                    dataGridView1.Rows[rowIndex].Cells[1].Value = newOrder.OrderNumber;
                    dataGridView1.Rows[rowIndex].Cells[2].Value = newOrder.CustomerName;
                    dataGridView1.Rows[rowIndex].Cells[3].Value = newOrder.TotalAmount.ToString("F2");

                    // Ĭ��ѡ�е�һ�в���ʾ��ϸ�����ڵ�һ�����ʱ��
                    if (orders.Count == 1)
                    {
                        dataGridView1.Rows[0].Selected = true;
                        DisplayOrderDetails(orders[0]);
                    }

                    SaveOrders(); // ���涩�����ļ�
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
            dataGridView2.Rows.Clear(); // ���������ϸ
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

        //�޸Ķ���
        private void button3_Click(object sender, EventArgs e)
        {
            // ����Ƿ���ѡ����
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫ�޸ĵĶ�����");
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow.Cells[1].Value == null || string.IsNullOrWhiteSpace(selectedRow.Cells[1].Value.ToString()))
            {
                MessageBox.Show("ѡ�еĶ�����ϢΪ�գ���ѡ����Ч������");
                return;
            }

            int selectedIndex = selectedRow.Index;
            Order selectedOrder = orders[selectedIndex];

            // ���� Form2 ʵ������������
            using (Form2 form2 = new Form2())
            {
                // ��ѡ�ж������ݴ��� Form2
                form2.OrderNumber = selectedOrder.OrderNumber;
                form2.CustomerName = selectedOrder.CustomerName;
                form2.TotalAmount = selectedOrder.TotalAmount.ToString("F2");
                foreach (var item in selectedOrder.Details)
                {
                    form2.OrderDetails.Rows.Add(item.ItemName, item.Price.ToString("F2"), item.Quantity.ToString("F0"));
                }

                if (form2.ShowDialog() == DialogResult.OK)
                {
                    // ���¶�������
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

                    // ���� dataGridView1
                    dataGridView1.Rows[selectedIndex].Cells[1].Value = selectedOrder.OrderNumber;
                    dataGridView1.Rows[selectedIndex].Cells[2].Value = selectedOrder.CustomerName;
                    dataGridView1.Rows[selectedIndex].Cells[3].Value = selectedOrder.TotalAmount.ToString("F2");

                    // ���� dataGridView2
                    DisplayOrderDetails(selectedOrder);

                    // ���浽�ļ�
                    SaveOrders();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // ����Ƿ���ѡ����
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("��ѡ��Ҫɾ���Ķ�����");
                return;
            }

            // �ж�ѡ�����Ƿ�Ϊ��
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            if (selectedRow.Cells[1].Value == null || string.IsNullOrWhiteSpace(selectedRow.Cells[1].Value.ToString()))
            {
                MessageBox.Show("ѡ�еĶ�����ϢΪ�գ���ѡ����Ч������");
                return;
            }

            // ��ȡѡ���е�����
            int selectedIndex = selectedRow.Index;
            string? orderNumber = selectedRow.Cells[1].Value.ToString();

            // ����ȷ�϶Ի���
            DialogResult result = MessageBox.Show($"��ȷ��Ҫɾ�����Ϊ {orderNumber} �Ķ�����",
                                                  "ȷ��ɾ��",
                                                  MessageBoxButtons.OKCancel,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                // �� orders �б���ɾ����Ӧ����
                orders.RemoveAt(selectedIndex);

                // �� dataGridView1 ��ɾ��ѡ����
                dataGridView1.Rows.RemoveAt(selectedIndex);

                // ������ţ�ȷ����������ǰ������
                int rowCount = dataGridView1.Rows.Count;
                for (int i = 0; i < rowCount; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = i + 1;
                }

                // ���� dataGridView2 ��ʾ
                if (orders.Count > 0)
                {
                    // Ĭ��ѡ�е�һ�в���ʾ����ϸ
                    dataGridView1.Rows[0].Selected = true;
                    DisplayOrderDetails(orders[0]);
                }
                else
                {
                    // �޶���ʱ��� dataGridView2
                    dataGridView2.Rows.Clear();
                }
            }
        }
    }
}
