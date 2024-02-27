using System;
using System.Threading;

namespace HelloEvent3
{

    /// <summary>
    /// 进阶版语法糖的自定义事件例子：选用了通用类型EventHandler
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            Waiter waiter = new Waiter();
            customer.Order += waiter.action;
            customer.Action();
            customer.PayBill();

        }
    }
    //事件访问内容（点的菜单和大小）
    //当你需要在事件发生时传递一些数据，你可以创建一个从 EventArgs 派生的类，这个一般默认继承系统的
    public class OrderEventArgs : EventArgs
    {
        public string DishName;
        public string Size;
    }

    //声明委托类型（注意如果这个委托是为了事件声明的这个命名方式是事件名字+EventHandle）
    //public delegate void OrderEventHandle(Customer customer, OrderEventArgs e);

    public class Customer
    {
        //声明事件方式(这里用了厂商提供通用的事件委托EventHandler， 不需要我们再去声明一个委托类型)
        public event EventHandler Order;

        public double Bill { get; set; }
        public void PayBill()
        {
            Console.WriteLine("I will pay ${0}", this.Bill);
        }
        public void WalkIn()
        {
            Console.WriteLine("Walk into the reastaurant");
        }
        public void SitDown()
        {
            Console.WriteLine("Sit down");
        }
        public void Think()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("let me think ...");
                Thread.Sleep(1000);
            }
            //触发事件
            if (this.Order != null)
            {
                OrderEventArgs e = new OrderEventArgs();
                e.DishName = "Kongpao Chicken";
                e.Size = "large";
                this.Order.Invoke(this, e);
            }
        }
        public void Action()
        {
            Console.ReadLine();
            this.WalkIn();
            this.SitDown();
            this.Think();
        }
    }

    //事件响应者
    public class Waiter
    {
        internal void action(object sender, EventArgs e)
        {
            Customer customer = sender as Customer;
            OrderEventArgs orderEventArgs = e as OrderEventArgs;
            Console.WriteLine("I will serve you the dish - {0}", orderEventArgs.DishName);
            double price = 10;
            switch (orderEventArgs.Size)
            {
                case "small":
                    price = price * 0.5;
                    break;
                case "large":
                    price = price * 1.5;
                    break;
                default:
                    break;
            }
            customer.Bill += price;
        }
    }
}
