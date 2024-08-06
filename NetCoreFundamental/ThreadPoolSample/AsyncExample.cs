namespace ThreadPoolSample
{
    public class AsyncExample
    {
        public static async Task MainTask()
        {
            Console.WriteLine("Current Thread: " + Environment.CurrentManagedThreadId);
            await SubTask1();
            Console.WriteLine("Current Thread: " + Environment.CurrentManagedThreadId);
        }

        public static async Task SubTask1()
        {
            Console.WriteLine("Current Thread in SubTask1: " + Environment.CurrentManagedThreadId);
            await Task.Delay(1000);
        }
    }
}