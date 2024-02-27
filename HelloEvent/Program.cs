using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelloEvent
{
    /// <summary>
    /// 这是完整版自定义事件的例子
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
    public delegate void OrderEventHandle(Customer customer, OrderEventArgs e);

    public class Customer
    {
        //委托字段，注意是存储和引用事件处理器
        private OrderEventHandle orderEventHandle;

        //声明事件方式
        public event OrderEventHandle Order
        {
            add { this.orderEventHandle += value; }
            remove { this.orderEventHandle -= value; }
        }
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
            if (this.orderEventHandle != null)
            {
                OrderEventArgs e = new OrderEventArgs();
                e.DishName = "Kongpao Chicken";
                e.Size = "large";
                this.orderEventHandle.Invoke(this, e);
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
        internal void action(Customer customer, OrderEventArgs e)
        {
            Console.WriteLine("I will serve you the dish - {0}", e.DishName);
            double price = 10;
            switch (e.Size)
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
