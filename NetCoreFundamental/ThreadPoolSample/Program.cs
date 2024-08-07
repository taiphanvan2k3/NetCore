namespace ThreadPoolSample
{
    public class Program
    {
        static void ThreadPoolExamples()
        {
            for (int i = 1; i < 100; i++)
            {
                int capturedI = i;
                MyThreadPool.QueueUserWorkItem(() =>
                {
                    Console.WriteLine($"Callback called: {capturedI} by (thread #{Environment.CurrentManagedThreadId})");
                });
            }
            Console.ReadLine();
        }

        static async Task CancellationExamples()
        {
            await CancellationExample.Example2();
        }

        static async Task TaskExamples()
        {
            await Task.Run(TaskExample.CalculateTime);
            await TaskExample.CalculateTime02();
            await TaskExample.CalculateTime03();
            TaskExample.CalculateTime04();
        }

        static async Task Main(string[] args)
        {
            await TaskExamples();
        }
    }
}