namespace AsyncAwait
{
    public class Example
    {
        public async Task _ExampleAsync()
        {
            // Tốn tổng cộng 9s
            DateTime start = DateTime.Now;
            Task task1 = GetData1();
            Task task2 = GetData2();

            await Task.WhenAll(task1, task2);
            DateTime end = DateTime.Now;
            Console.WriteLine("Total time:" + end.Subtract(start).ToString(""));
            Console.WriteLine("Both requests completed");
        }

        public async Task _ExampleAsync2()
        {
            // Tốn tổng cộng 12s
            DateTime start = DateTime.Now;
            await GetData1();
            await GetData2();

            DateTime end = DateTime.Now;
            Console.WriteLine("Total time:" + end.Subtract(start).ToString(""));
            Console.WriteLine("Both requests completed");
        }

        public Task _ExampleAsync3()
        {
            DateTime start = DateTime.Now;
            Task t1 = GetData3();
            for (int i = 1; i <= 100; i++)
            {
                Console.Write(i + " ");
            }
            System.Console.WriteLine();
            t1.Wait();
            // await GetData3();
            for (int i = 1; i <= 1000; i++)
            {
                Console.Write(i + " ");
            }
            DateTime end = DateTime.Now;
            Console.WriteLine("\nTotal time:" + end.Subtract(start).ToString(""));
            Console.WriteLine("Both requests completed");
            return Task.CompletedTask;
        }

        async Task GetData1()
        {
            await Task.Delay(9000); // Giả định một công việc bất đồng bộ
            Console.WriteLine("Data 1 loaded");
        }

        async Task GetData2()
        {
            await Task.Delay(3000); // Giả định một công việc bất đồng bộ
            Console.WriteLine("Data 2 loaded");
        }

        async Task GetData3()
        {
            Console.WriteLine("Task start");
            await Task.Delay(3000); // Giả định một công việc bất đồng bộ
            Console.WriteLine("Data 2 loaded");
        }
    }
}