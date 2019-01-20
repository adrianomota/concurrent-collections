using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Program
    {
        public static readonly List<string> AllShirtNames =
              new List<string> { "technologyhour", "Code School", "jDays", "buddhistgeeks", "iGeek" };

        private static void Main(string[] args)
        {
            InitializeStack();
            Console.ReadKey();
        }

        private static void InitializeStack()
        {
            var shirts = new ConcurrentStack<string>();
            shirts.Push("Pluralsight");
            shirts.Push("WordPress");
            shirts.Push("Code School");

            Console.WriteLine($"After enqueuing count = {shirts.Count}");

            //string item1 = shirts.Dequeue();
            var success = shirts.TryPop(out string item1);

            if (success)
            {
                Console.WriteLine($"\r\nRemoving {item1}");
            }
            else
            {
                Console.WriteLine($"queue empty");
            }

            success = shirts.TryPeek(out string item2);

            if (success)
            {
                Console.WriteLine($"\r\nPeeking {item2}");
            }
            else
            {
                Console.WriteLine($"queue empty");
            }

            Console.WriteLine($"\r\nEnumerating");

            foreach (var item in shirts)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"After enumerating count = {shirts.Count}");
        }

        private static void InitializeQueue()
        {
            var shirts = new ConcurrentQueue<string>();
            shirts.Enqueue("Pluralsight");
            shirts.Enqueue("WordPress");
            shirts.Enqueue("Code School");

            Console.WriteLine($"After enqueuing count = {shirts.Count}");

            //string item1 = shirts.Dequeue();
            var success = shirts.TryDequeue(out string item1);

            if (success)
            {
                Console.WriteLine($"\r\nRemoving {item1}");
            }
            else
            {
                Console.WriteLine($"queue empty");
            }

            success = shirts.TryPeek(out string item2);

            if (success)
            {
                Console.WriteLine($"\r\nPeeking {item2}");
            }
            else
            {
                Console.WriteLine($"queue empty");
            }

            Console.WriteLine($"\r\nEnumerating");

            foreach (var item in shirts)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"After enumerating count = {shirts.Count}");
        }

        private static void InitializeWithConcurrentDictionary()
        {
            StockController controller = new StockController();
            TimeSpan workDay = new TimeSpan(0, 0, 2);

            Task t1 = Task.Run(() => new SalesPerson("Sahil").Work(controller, workDay));
            Task t2 = Task.Run(() => new SalesPerson("Peter").Work(controller, workDay));
            Task t3 = Task.Run(() => new SalesPerson("Juliette").Work(controller, workDay));
            Task t4 = Task.Run(() => new SalesPerson("Xavier").Work(controller, workDay));

            Task.WaitAll(t1, t2, t3, t4);
            controller.DisplayStatus();
        }

        private static void RunDictionary()
        {
            var stock = new ConcurrentDictionary<string, int>();
            stock.TryAdd("jDays", 4);
            stock.TryAdd("technologyhour", 3);

            Console.WriteLine($"No. of shirts in stock = {stock.Count}");

            bool success = stock.TryAdd("pluralsight", 6);
            Console.WriteLine($"Added sucessed?{success}");
            success = stock.TryAdd("pluralsight", 6);
            Console.WriteLine($"Added sucessed?{success}");

            stock.GetOrAdd("buddhistgeeks", 5);

            //stock["pluralsight"] = 7;

            int psStock = stock.AddOrUpdate("pluralsight", 1, (key, oldValue) => oldValue + 1);
            Console.WriteLine($"New value: {psStock}");

            success = stock.TryRemove("jDays", out int jDaysValue);
            if (success)
            {
                Console.WriteLine($"value removed was: {jDaysValue}");
            }

            Console.WriteLine("\r\n=====Enumerating======");
            foreach (var keyValuePair in stock)
            {
                Console.WriteLine($"{keyValuePair.Key}: {keyValuePair.Value}");
            }
        }

        private static void RunQueue()
        {
            var orders = new Queue<string>();

            //PLaceOrders(orders, "Adriano");
            //PLaceOrders(orders, "Miguel");

            Task task1 = Task.Run(() => PLaceOrders(orders, "Adriano"));
            Task task2 = Task.Run(() => PLaceOrders(orders, "Miguel"));

            Task.WaitAll(task1, task2);

            foreach (var order in orders)
            {
                Console.WriteLine($"{order}");
            }
        }

        private static readonly object _lockObj = new object();

        private static void PLaceOrders(Queue<string> orders, string customerName)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1);
                string orderName = $"{customerName} wants t-shirt {i + 1}";
                lock (_lockObj)
                {
                    orders.Enqueue(orderName);
                }
            }
        }
    }
}