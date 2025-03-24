using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderManagementConsole
{

    //商品类
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Product(string name, double price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString() => $"{Name} (${Price})";
    }
    //客户类
    public class Customer
    {
        public string Name { get; set; }

        public Customer(string name) => Name = name;

        public override string ToString() => Name;
    }
    //订单明细类
    public class OrderDetails
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice => Product.Price * Quantity;

        public OrderDetails(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if(obj is  OrderDetails other)
            {
                return Product.Name == other.Product.Name;
            }
            return false;
            
        }

        public override int GetHashCode() => Product.Name.GetHashCode();

        public override string ToString() => $"{Product} × {Quantity} = ${TotalPrice}";
    }
    //订单类
    public class Order
    {
        public int OrderId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderDetails> Details { get; set; } = new List<OrderDetails>();
        public double TotalAmount => Details.Sum(d => d.TotalPrice);

        public Order(int orderId, Customer customer)
        {
            OrderId = orderId;
            Customer = customer;
        }

        public void AddDetail(OrderDetails detail)
        {
            if (!Details.Contains(detail))
                Details.Add(detail);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is Order other)
            {
                return OrderId == other.OrderId;
            }
                return false;
            
        }

        public override int GetHashCode() => OrderId.GetHashCode();

        public override string ToString() =>
            $"订单 #{OrderId}\n客户: {Customer}\n明细:\n" +
            $"{string.Join("\n", Details.Select(d => "  " + d))}\n总金额: ${TotalAmount}";
    }
    //订单服务类
    public class OrderService
    {
        private List<Order> orders = new List<Order>();

        public void AddOrder(Order order)
        {
            if (orders.Contains(order))
                throw new ArgumentException($"订单 #{order.OrderId} 已存在！");
            orders.Add(order);
        }

        public void RemoveOrder(int orderId)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new ArgumentException($"订单 #{orderId} 未找到！");
            orders.Remove(order);
        }

        public void UpdateOrder(int orderId, Order updatedOrder)
        {
            var order = orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                throw new ArgumentException($"订单 #{orderId} 未找到！");
            order.Customer = updatedOrder.Customer;
            order.Details.Clear();
            updatedOrder.Details.ForEach(d => order.AddDetail(d));
        }
        
        //不同方式实现查询功能
        public List<Order> QueryByOrderId(int orderId) =>
            orders.Where(o => o.OrderId == orderId)
                  .OrderBy(o => o.TotalAmount)
                  .ToList();

        public List<Order> QueryByProductName(string productName) =>
            orders.Where(o => o.Details.Any(d => d.Product.Name.Contains(productName)))
                  .OrderBy(o => o.TotalAmount)
                  .ToList();

        public List<Order> QueryByCustomer(string customerName) =>
            orders.Where(o => o.Customer.Name.Contains(customerName))
                  .OrderBy(o => o.TotalAmount)
                  .ToList();

        public List<Order> QueryByAmount(double minAmount, double maxAmount) =>
            orders.Where(o => o.TotalAmount >= minAmount && o.TotalAmount <= maxAmount)
                  .OrderBy(o => o.TotalAmount)
                  .ToList();

        public void SortOrders(Func<Order, object>? keySelector = null)
        {
            if (keySelector == null)
            {
                orders.Sort((a, b) => a.OrderId.CompareTo(b.OrderId));
            }
            else
            {
                orders.Sort((a, b) => Comparer<object>.Create((x, y) =>
                    (x?.ToString() ?? "").CompareTo(y?.ToString() ?? ""))
                    .Compare(keySelector(a), keySelector(b)));
            }
        }

        public List<Order> GetAllOrders() => orders;
    }
    //主程序
    internal class Program
    {
        static OrderService orderService = new OrderService();

        static void Main(string[] args)
        {
            InitializeTestOrders();//初始化五个订单
            OrderServiceTests.RunAllTests(); // 运行所有测试
            Console.WriteLine("测试完成，按任意键继续...");
            Console.ReadKey(); // 暂停，等待用户按键

            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1": AddOrder(); break;
                    case "2": RemoveOrder(); break;
                    case "3": UpdateOrder(); break;
                    case "4": QueryOrders(); break;
                    case "5": SortAndDisplay(); break;
                    case "6": return;
                    default:
                        Console.WriteLine("无效选项！按任意键返回...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void InitializeTestOrders()
        {
            var order1 = new Order(1, new Customer("张三"));
            order1.AddDetail(new OrderDetails(new Product("书", 10), 2));
            order1.AddDetail(new OrderDetails(new Product("笔", 5), 3));
            orderService.AddOrder(order1);

            var order2 = new Order(2, new Customer("李四"));
            order2.AddDetail(new OrderDetails(new Product("笔记本电脑", 1000), 1));
            orderService.AddOrder(order2);

            var order3 = new Order(3, new Customer("王五"));
            order3.AddDetail(new OrderDetails(new Product("鼠标", 20), 5));
            orderService.AddOrder(order3);

            var order4 = new Order(4, new Customer("赵六"));
            order4.AddDetail(new OrderDetails(new Product("键盘", 50), 2));
            order4.AddDetail(new OrderDetails(new Product("显示器", 300), 1));
            orderService.AddOrder(order4);

            var order5 = new Order(5, new Customer("孙七"));
            order5.AddDetail(new OrderDetails(new Product("耳机", 80), 1));
            orderService.AddOrder(order5);
        }

        static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=== 订单管理系统 ===");
            Console.WriteLine("1. 添加订单");
            Console.WriteLine("2. 删除订单");
            Console.WriteLine("3. 修改订单");
            Console.WriteLine("4. 查询订单");
            Console.WriteLine("5. 排序并显示所有订单");
            Console.WriteLine("6. 退出");
            Console.Write("请选择一个选项：");
        }

        static void DisplayOrdersTable(List<Order> orders)
        {
            if (orders.Count == 0)
            {
                Console.WriteLine("无订单可显示！");
                return;
            }

            Console.WriteLine("\n订单列表：");
            foreach (var order in orders)
            {
                Console.WriteLine("+--------+----------------+------------+");
                Console.WriteLine("| 订单ID | 客户姓名       | 总金额     |");
                Console.WriteLine("+--------+----------------+------------+");
                Console.WriteLine($"| {order.OrderId,-6} | {order.Customer.Name,-14} | {order.TotalAmount,10:F2} |");
                Console.WriteLine("+--------+----------------+------------+");

                if (order.Details.Any())
                {
                    Console.WriteLine("  订单明细：");
                    Console.WriteLine("  +----------------+----------+----------+------------+");
                    Console.WriteLine("  | 商品名称       | 价格     | 数量     | 小计       |");
                    Console.WriteLine("  +----------------+----------+----------+------------+");
                    foreach (var detail in order.Details)
                    {
                        Console.WriteLine($"  | {detail.Product.Name,-14} | {detail.Product.Price,8:F2} | {detail.Quantity,8} | {detail.TotalPrice,10:F2} |");
                    }
                    Console.WriteLine("  +----------------+----------+----------+------------+");
                }
                Console.WriteLine(); // 订单间空行分隔
            }
        }

        static void DisplayOrdersSimpleTable(List<Order> orders)
        {
            if (orders.Count == 0)
            {
                Console.WriteLine("当前没有订单！");
                return;
            }

            Console.WriteLine("\n订单列表：");
            Console.WriteLine("+----+----------------+------------+");
            Console.WriteLine("| 序号 | 订单ID        | 客户姓名   |");
            Console.WriteLine("+----+----------------+------------+");
            for (int i = 0; i < orders.Count; i++)
            {
                Console.WriteLine($"| {i,-4} | {orders[i].OrderId,-14} | {orders[i].Customer.Name,-10} |");
            }
            Console.WriteLine("+----+----------------+------------+");
        }

        static void AddOrder()
        {
            Console.Clear();
            try
            {
                Console.Write("请输入订单ID：");
                int id = int.Parse(Console.ReadLine() ?? "0");
                Console.Write("请输入客户姓名：");
                var customer = new Customer(Console.ReadLine() ?? "未知");
                var order = new Order(id, customer);

                while (true)
                {
                    Console.Write("请输入商品名称（输入'done'结束）：");
                    string name = Console.ReadLine() ?? "";
                    if (name.ToLower() == "done") break;
                    Console.Write("请输入价格：");
                    double price = double.Parse(Console.ReadLine() ?? "0");
                    Console.Write("请输入数量：");
                    int qty = int.Parse(Console.ReadLine() ?? "0");
                    order.AddDetail(new OrderDetails(new Product(name, price), qty));
                }
                orderService.AddOrder(order);
                Console.WriteLine("订单添加成功！按任意键返回...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误：{ex.Message} 按任意键返回...");
            }
            Console.ReadKey();
        }

        static void RemoveOrder()
        {
            Console.Clear();
            var orders = orderService.GetAllOrders();
            DisplayOrdersSimpleTable(orders);

            if (orders.Count == 0)
            {
                Console.WriteLine("按任意键返回...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.Write("请输入要删除的订单序号（0到{0}）：", orders.Count - 1);
                int index = int.Parse(Console.ReadLine() ?? "-1");
                if (index < 0 || index >= orders.Count)
                    throw new ArgumentException("序号超出范围！");

                int orderId = orders[index].OrderId;
                orderService.RemoveOrder(orderId);
                Console.WriteLine("订单删除成功！按任意键返回...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误：{ex.Message} 按任意键返回...");
            }
            Console.ReadKey();
        }

        static void UpdateOrder()
        {
            Console.Clear();
            var orders = orderService.GetAllOrders();
            DisplayOrdersSimpleTable(orders);

            if (orders.Count == 0)
            {
                Console.WriteLine("按任意键返回...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.Write("请输入要修改的订单序号（0到{0}）：", orders.Count - 1);
                int index = int.Parse(Console.ReadLine() ?? "-1");
                if (index < 0 || index >= orders.Count)
                    throw new ArgumentException("序号超出范围！");

                int orderId = orders[index].OrderId;
                Console.Write("请输入新客户姓名：");
                var customer = new Customer(Console.ReadLine() ?? "未知");
                var order = new Order(orderId, customer);

                while (true)
                {
                    Console.Write("请输入商品名称（输入'done'结束）：");
                    string name = Console.ReadLine() ?? "";
                    if (name.ToLower() == "done") break;
                    Console.Write("请输入价格：");
                    double price = double.Parse(Console.ReadLine() ?? "0");
                    Console.Write("请输入数量：");
                    int qty = int.Parse(Console.ReadLine() ?? "0");
                    order.AddDetail(new OrderDetails(new Product(name, price), qty));
                }
                orderService.UpdateOrder(orderId, order);
                Console.WriteLine("订单修改成功！按任意键返回...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"错误：{ex.Message} 按任意键返回...");
            }
            Console.ReadKey();
        }

        static void QueryOrders()
        {
            Console.Clear();
            Console.WriteLine("1. 按订单ID查询");
            Console.WriteLine("2. 按商品名称查询");
            Console.WriteLine("3. 按客户查询");
            Console.WriteLine("4. 按金额范围查询");
            Console.Write("请选择查询类型：");

            var results = new List<Order>();
            switch (Console.ReadLine() ?? "")
            {
                case "1":
                    Console.Write("请输入订单ID：");
                    results = orderService.QueryByOrderId(int.Parse(Console.ReadLine() ?? "0"));
                    break;
                case "2":
                    Console.Write("请输入商品名称：");
                    results = orderService.QueryByProductName(Console.ReadLine() ?? "");
                    break;
                case "3":
                    Console.Write("请输入客户姓名：");
                    results = orderService.QueryByCustomer(Console.ReadLine() ?? "");
                    break;
                case "4":
                    Console.Write("请输入最低金额：");
                    double min = double.Parse(Console.ReadLine() ?? "0");
                    Console.Write("请输入最高金额：");
                    double max = double.Parse(Console.ReadLine() ?? "0");
                    results = orderService.QueryByAmount(min, max);
                    break;
            }
            DisplayOrdersTable(results);
            Console.WriteLine("按任意键返回...");
            Console.ReadKey();
        }

        static void SortAndDisplay()
        {
            Console.Clear();
            Console.WriteLine("1. 默认（按订单ID）");
            Console.WriteLine("2. 按总金额");
            Console.Write("请选择排序类型：");
            switch (Console.ReadLine() ?? "")
            {
                case "1": orderService.SortOrders(); break;
                case "2": orderService.SortOrders(o => o.TotalAmount); break;
            }
            DisplayOrdersTable(orderService.GetAllOrders());
            Console.WriteLine("按任意键返回...");
            Console.ReadKey();
        }
    }

    public class OrderServiceTests
    {
        private OrderService service;

        public OrderServiceTests() => service = new OrderService();

        public void TestAddOrder()
        {
            var order = new Order(1, new Customer("张三"));
            order.AddDetail(new OrderDetails(new Product("书", 10), 2));
            service.AddOrder(order);
            Console.WriteLine("TestAddOrder: " +
                (service.QueryByOrderId(1).Count == 1 ? "通过" : "失败"));
        }

        public void TestRemoveOrder()
        {
            try
            {
                var order = new Order(2, new Customer("李四"));
                order.AddDetail(new OrderDetails(new Product("笔", 5), 3));
                service.AddOrder(order);
                service.RemoveOrder(2);
                Console.WriteLine("TestRemoveOrder: " +
                    (service.QueryByOrderId(2).Count == 0 ? "通过" : "失败"));
            }
            catch { Console.WriteLine("TestRemoveOrder: 失败"); }
        }

        public void TestUpdateOrder()
        {
            try
            {
                var order = new Order(3, new Customer("王五"));
                service.AddOrder(order);
                var newOrder = new Order(3, new Customer("赵六"));
                newOrder.AddDetail(new OrderDetails(new Product("鼠标", 20), 1));
                service.UpdateOrder(3, newOrder);
                var updated = service.QueryByOrderId(3)[0];
                Console.WriteLine("TestUpdateOrder: " +
                    (updated.Customer.Name == "赵六" && updated.Details.Count == 1 ? "通过" : "失败"));
            }
            catch { Console.WriteLine("TestUpdateOrder: 失败"); }
        }

        public void TestQuery()
        {
            service = new OrderService(); // 重置 service
            var order = new Order(4, new Customer("孙七"));
            order.AddDetail(new OrderDetails(new Product("键盘", 50), 1));
            service.AddOrder(order);
            bool passed = service.QueryByProductName("键盘").Count == 1 &&
                         service.QueryByCustomer("孙七").Count == 1 &&
                         service.QueryByAmount(0, 100).Count == 1;
            Console.WriteLine("TestQuery: " + (passed ? "通过" : "失败"));
        }

        public static void RunAllTests()
        {
            var tests = new OrderServiceTests();
            tests.TestAddOrder();
            tests.TestRemoveOrder();
            tests.TestUpdateOrder();
            tests.TestQuery();
        }
    }

}
