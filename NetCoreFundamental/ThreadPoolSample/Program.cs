namespace ThreadPoolSample
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // for (int i = 1; i < 100; i++)
            // {
            //     int capturedI = i;
            //     MyThreadPool.QueueUserWorkItem(() =>
            //     {
            //         Console.WriteLine($"Callback called: {capturedI} by (thread #{Environment.CurrentManagedThreadId})");
            //     });
            // }
            // Console.ReadLine();

            await AsyncExample.MainTask();
        }
    }
}