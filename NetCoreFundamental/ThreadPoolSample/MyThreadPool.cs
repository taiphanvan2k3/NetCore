using System.Collections.Concurrent;

namespace ThreadPoolSample;

public class MyThreadPool
{
    private static ulong currentId = 0;

    // BlockingCollection là 1 thread safe collection, nó sẽ block thread nếu collection đang rỗng hoặc đang full
    private static readonly BlockingCollection<(ulong, Action, ExecutionContext)> actions = [];
    public static void QueueUserWorkItem(Action action) => actions.Add((Interlocked.Increment(ref currentId), action, ExecutionContext.Capture()));

    /// <summary>
    /// Static constructor is called only once per application domain.
    /// </summary>
    static MyThreadPool()
    {
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            // Tạo ra các thread worker, mỗi thread sẽ lấy 1 phần tử từ BlockingCollection và thực hiện công việc đó
            var t = new Thread(() =>
            {
                Console.WriteLine($"Thread #{Environment.CurrentManagedThreadId} started!");

                while (true)
                {
                    // Lấy 1 phần tử từ BlockingCollection, nếu BlockingCollection đang rỗng thì thread sẽ bị block cho đến khi có phần tử mới
                    // Mỗi phần tử bao gồm các thông tin: id, công việc cần thực hiện, và context
                    (ulong id, Action action, ExecutionContext context) = actions.Take();
                    Console.WriteLine($"Thread #{Environment.CurrentManagedThreadId} executes task #{id}");

                    if (context is null)
                    {
                        action();
                    }
                    else
                    {
                        ExecutionContext.Run(context, state => ((Action)state!).Invoke(), action);
                    }
                }
            })
            {
                IsBackground = true
            };
            t.Start();
        }
    }

    public static void Test()
    {

    }
}