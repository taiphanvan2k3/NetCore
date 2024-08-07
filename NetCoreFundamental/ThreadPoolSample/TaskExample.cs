using System.Diagnostics;

namespace ThreadPoolSample
{
    public class TaskExample
    {
        private static void SyncTask()
        {
            Console.WriteLine("SyncTask01");
            Thread.Sleep(2000);
        }

        private static void SyncTask2()
        {
            Console.WriteLine("SyncTask02");
            Thread.Sleep(1500);
        }

        private static Task AsyncTask()
        {
            Console.WriteLine("AsyncTask01");
            return Task.Delay(2000);
        }

        public static void CalculateTime()
        {
            Console.WriteLine("-> CalculateTime");
            var sw = Stopwatch.StartNew();
            SyncTask();
            SyncTask2();

            // Trong ví dụ này tổng thời gian chạy sẽ là tổng thời gian của 2 task, vì chúng chạy tuần tự
            Console.WriteLine($"-> Total time: {sw.ElapsedMilliseconds}ms");
        }

        public static async Task CalculateTime02()
        {
            Console.WriteLine("-> CalculateTime02");
            var sw = Stopwatch.StartNew();

            // Tuy 2 task là sync nhưng được chạy trên 2 thread khác nhau
            // do đó tổng thời gian chạy sẽ giảm đi so với việc chạy tuần tự
            var (task1, task2) = (Task.Run(SyncTask), Task.Run(SyncTask2));

            await Task.WhenAll(task1, task2);
            Console.WriteLine($"-> Total time: {sw.ElapsedMilliseconds}ms");
        }

        public static async Task CalculateTime03()
        {
            Console.WriteLine("-> CalculateTime03");
            var sw = Stopwatch.StartNew();
            var (task1, task2) = (Task.Run(SyncTask), AsyncTask());

            await Task.WhenAll(task1, task2);
            Console.WriteLine($"-> Total time: {sw.ElapsedMilliseconds}ms");
        }

        public static void CalculateTime04()
        {
            Console.WriteLine("-> CalculateTime04");
            var sw = Stopwatch.StartNew();

            // Tuy 2 task là sync nhưng được chạy trên 2 thread khác nhau
            // do đó tổng thời gian chạy sẽ giảm đi so với việc chạy tuần tự
            var (task1, task2) = (Task.Run(SyncTask), Task.Run(SyncTask2));

            Task.WaitAll(task1, task2);
            Console.WriteLine($"-> Total time: {sw.ElapsedMilliseconds}ms");
        }
    }
}