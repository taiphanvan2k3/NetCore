using AsyncAwait;

namespace Netcore
{
    public class Program
    {
        static void DoSomeThing(int seconds, string message, ConsoleColor color)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = color;
                System.Console.WriteLine($"{message}  ...Start");
                Console.ResetColor();
            }

            for (int i = 1; i <= seconds; i++)
            {
                // Khóa thuộc tính Out này lại, phải đợi thực hiện xong
                // 3 tác vụ bên trong thì mới mở khóa ra để các luồng khác có thể truy cập
                lock (Console.Out)
                {
                    Console.ForegroundColor = color;
                    System.Console.WriteLine($"{message,10} {i,2}");
                    Console.ResetColor();
                }
                Thread.Sleep(1000);
            }

            lock (Console.Out)
            {
                Console.ForegroundColor = color;
                System.Console.WriteLine($"{message}  ...end");
                Console.ResetColor();
            }
        }

        private static void InitTask_1()
        {
            Task t2 = new Task(
                () =>
                {
                    DoSomeThing(10, "T2", ConsoleColor.Green);
                }
            );

            Task t3 = new Task(
                (object obj) =>
                {
                    string nameTask = obj as string;
                    DoSomeThing(8, nameTask, ConsoleColor.Blue);
                }, "T3");

            DoSomeThing(6, "T1", ConsoleColor.Red); // chạy trên 1 thread
            t2.Start(); // chạy trên 1 thread
            t3.Start(); // chạy trên 1 thread

            // T1 là task chính và nó nằm trong luồng chính. Khi T1 thực hiện xong thì nó sẽ kết thúc hàm nên t2, t3 không được thực hiện
            // Để khắc phụ có thể đưa t2.Start() và t3.Start() lên trước DoSomeThing hoặc thêm Console.ReadKey() để cho dù làm xong DoSomeThing
            // thì hàm này vẫn chưa kết thúc và t2, t3 sẽ được thực hiện.
        }

        private static void InitTask_2()
        {
            Task t2 = new Task(
                () =>
                {
                    DoSomeThing(10, "T2", ConsoleColor.Green);
                }
            );

            Task t3 = new Task(
                (object obj) =>
                {
                    string nameTask = obj as string;
                    DoSomeThing(8, nameTask, ConsoleColor.Blue);
                }, "T3");

            t2.Start(); // chạy trên 1 thread
            t3.Start(); // chạy trên 1 thread
            DoSomeThing(20, "T1", ConsoleColor.Red); // chạy trên 1 thread
            // t2, t3 có thể chạy sau khi T1 chạy for vòng đầu. Do t1 chạy số vòng lặp nhiều nhất nên sẽ bị kết thúc cuối
            // cùng nên điều này giúp cho t2,t3 được chạy hết. Nếu t1 mà kết thúc trước tức là luồng chính kết thúc thì hàm kết
            // thúc luôn => t2,t3 không chạy được nữa
        }

        private static void InitTask_3()
        {
            Task t2 = new Task(
                () =>
                {
                    DoSomeThing(10, "T2", ConsoleColor.Green);
                }
            );

            Task t3 = new Task(
                (object obj) =>
                {
                    string nameTask = obj as string;
                    DoSomeThing(8, nameTask, ConsoleColor.Blue);
                }, "T3");

            t2.Start(); // chạy trên 1 thread
            t3.Start(); // chạy trên 1 thread
            DoSomeThing(5, "T1", ConsoleColor.Red); // chạy trên 1 thread

            // Điều này đảm bảo rằng task t2,t3 đã thực hiện xong rồi mới in ra dòng chữ ở Console.WriteLine
            // Cho dù t1 thực hiện xong rồi nhưng khi đến gặp t2.Wait() và t3.Wait() thì nó phải đợi cho 2 task
            // làm xong trước khi in ra màn hình
            // t2.Wait();
            // t3.Wait();

            // Hoặc dùng:
            Task.WaitAll(t2, t3);
            System.Console.WriteLine("Hàm này đã thực hiện xong");
        }

        private static Task Task2()
        {
            Task t2 = new Task(
                () =>
                {
                    DoSomeThing(10, "T2", ConsoleColor.Green);
                }
            );
            t2.Start();

            return t2;
        }

        private static Task Task3()
        {
            Task t3 = new Task(
                (object obj) =>
                {
                    string nameTask = obj as string;
                    DoSomeThing(8, nameTask, ConsoleColor.Blue);
                }, "T3"
            );
            t3.Start();
            return t3;
        }

        private static void Vidu1()
        {
            Task t2 = Task2();
            Task t3 = Task3();
            DoSomeThing(5, "T1", ConsoleColor.Red); // chạy trên 1 thread
            Task.WaitAll(t2, t3);
            // Tương tự thì task chính chạy DoSomeThing(T1) luôn được chạy trước, trong lúc chạy nó khởi tạo
            // ra 2 task T2,T3
            System.Console.WriteLine("Done all");
        }

        private static async Task Task2_v2()
        {
            // Không cần return 
            Task t2 = new Task(
                () =>
                {
                    DoSomeThing(10, "T2", ConsoleColor.Green);
                }
            );
            t2.Start();

            // Mong muốn sau khi t2 được thực hiện xong sẽ in ra dòng chữ:
            // await cũng có cơ chế như wait nhưng khi gặp await thì đã return t2; rồi nên ko cần return nữa
            await t2;
            System.Console.WriteLine("T2 done!");
        }

        private static async Task Task3_v2()
        {
            Task t3 = new Task(
                (object obj) =>
                {
                    string nameTask = obj as string;
                    DoSomeThing(8, nameTask, ConsoleColor.Blue);
                }, "T3"
            );
            t3.Start();
            await t3;
            System.Console.WriteLine("T3 done!");
        }

        private static void Vidu2()
        {
            // Do có từ khóa await nên lúc này 
            Task t2 = Task2_v2();
            Task t3 = Task3_v2();
            DoSomeThing(5, "T1", ConsoleColor.Red); // chạy trên 1 thread
            Task.WaitAll(t2, t3);
            // Tương tự thì task chính chạy DoSomeThing(T1) luôn được chạy trước, trong lúc chạy nó khởi tạo
            // ra 2 task T2,T3
        }

        static async Task Main(string[] args)
        {
            // InitTask_3();
            // Vidu2();
            // Learn02.Demo1();

            // Một khi phươ
            // await Learn02.Demo3();
            // await Learn02.Demo4();
            await new Example()._ExampleAsync3();
        }
    }
}
