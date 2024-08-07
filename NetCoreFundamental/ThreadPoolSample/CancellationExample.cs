namespace ThreadPoolSample
{
    public class CancellationExample
    {
        static async Task DoWorkAsyncLoop(CancellationToken cancellationToken)
        {
            for (var idx = 0; idx < 10; idx++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine($"Thread: {Environment.CurrentManagedThreadId}, idx: {idx}");

                // Trong lúc await này thì Thread có thể đã được giải phóng để thực hiện công việc khác
                // và một Thread khác sẽ thực hiện tiếp công việc sau await
                await Task.Delay(2000, cancellationToken);
            }
        }

        static async Task DoWorkAsyncInfinite(CancellationToken cancellationToken)
        {
            int i = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine(i++);
                await Task.Delay(1000, cancellationToken);
            }
        }

        public static async Task Example1()
        {
            var cts = new CancellationTokenSource();
            await DoWorkAsyncLoop(cts.Token);

            // Câu lệnh này không còn tác dụng vì đã quá muộn, do thực hiện await nên đã chạy hết hàm DoWorkAsyncLoop
            cts.CancelAfter(5000);
            Console.WriteLine("Cancel after");
        }

        public static async Task Example2()
        {
            var cts = new CancellationTokenSource();
            Console.WriteLine($"Before DoWorkAsyncLoop with ThreadId: {Environment.CurrentManagedThreadId}");

            var task = DoWorkAsyncLoop(cts.Token);
            
            // task lúc này vẫn đang chạy, và ta sẽ đặt 1 lịch hẹn để cancel task sau 5s 
            cts.CancelAfter(5000);
            Console.WriteLine("Cancel after 5000ms");

            try
            {
                Console.WriteLine($"Before await: {Environment.CurrentManagedThreadId}");
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Task was cancelled: {Environment.CurrentManagedThreadId}");
            }
        }

        public static async Task Example3()
        {
            var cts = new CancellationTokenSource();
            var task = DoWorkAsyncInfinite(cts.Token);

            cts.CancelAfter(5000);
            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task was cancelled!");
            }
        }
    }
}