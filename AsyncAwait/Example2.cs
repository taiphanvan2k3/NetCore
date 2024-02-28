namespace NetCore.AsyncAwait
{
    public class Example2
    {
        // Chạy các hàm đồng bộ lần lượt => Tốn 12s
        public async Task Test()
        {
            DateTime start = DateTime.Now;
            SyncFunc1();
            SyncFunc2();
            Console.WriteLine("Spend: " + DateTime.Now.Subtract(start).ToString(""));
        }

        // Tạo task để chạy các hàm đồng bộ => Tốn max(8,4) = 8s và task t2 thực hiện xong trước
        public async Task Test2()
        {
            DateTime start = DateTime.Now;
            Task<string> t1 = Task.Run(SyncFunc1);
            Task<int> t2 = Task.Run(SyncFunc2);

            string res1 = await t1;
            int res2 = await t2;
            Console.WriteLine("Spend: " + DateTime.Now.Subtract(start).ToString(""));
            System.Console.WriteLine("kq: " + res1 + ", " + res2);
        }

        // Tạo task để chạy các hàm đồng bộ => Tốn max(8,4) = 8s 
        // Dùng WhenAll cũng cho ra cùng 1 thời gian và task t2 thực hiện xong trước
        public async Task Test3()
        {
            DateTime start = DateTime.Now;
            Task<string> t1 = Task.Run(SyncFunc1);
            Task<int> t2 = Task.Run(SyncFunc2);

            await Task.WhenAll(t1, t2);
            string res1 = await t1;
            int res2 = await t2;
            Console.WriteLine("Spend: " + DateTime.Now.Subtract(start).ToString(""));
            Console.WriteLine("kq: " + res1 + ", " + res2);
        }

        // Task t2 chỉ được chạy khi t1 thực hiện xong => Chạy lần lượt và tốn 12s
        public async Task Test4()
        {
            DateTime start = DateTime.Now;
            Task<string> t1 = Task.Run(SyncFunc1);
            string res1 = await t1;

            Task<int> t2 = Task.Run(SyncFunc2);
            int res2 = await t2;

            Console.WriteLine("Spend: " + DateTime.Now.Subtract(start).ToString(""));
            Console.WriteLine("kq: " + res1 + ", " + res2);
        }

        // Không nên dùng Task.WaitAll vì nó sẽ block thread => Freeze UI
        public async Task Test5()
        {
            DateTime start = DateTime.Now;
            Task<int> t2 = Task.Run(SyncFunc2);
            Task<string> t1 = Task.Run(SyncFunc1);

            Task.WaitAll(t1, t2);
            string res1 = t1.Result;
            int res2 = t2.Result;

            Console.WriteLine("Spend: " + DateTime.Now.Subtract(start).ToString(""));
            Console.WriteLine("kq: " + res1 + ", " + res2);
        }

        public string SyncFunc1()
        {
            Thread.Sleep(8000);
            Console.WriteLine("Done SyncFunc1");
            return "SyncFunc1";
        }

        public int SyncFunc2()
        {
            Thread.Sleep(4000);
            Console.WriteLine("Done SyncFunc2");
            return 1000;
        }
    }
}