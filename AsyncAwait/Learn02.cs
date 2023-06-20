namespace Netcore
{

    public class Learn02
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
                    System.Console.WriteLine($"{message,10} {i,2}. Id:" + Thread.CurrentThread.ManagedThreadId);
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

        private static Task<string> InitTask2()
        {
            Task<string> t2 = new Task<string>(
                () =>
                {
                    DoSomeThing(8, "T2", ConsoleColor.Blue);
                    return "Return from t3";
                }
            );
            t2.Start();
            return t2;
        }

        private static Task<string> InitTask3()
        {
            Task<string> t3 = new Task<string>(
                (object obj) =>
                {
                    string name = obj as string;
                    DoSomeThing(5, name, ConsoleColor.Blue);
                    return $"Return from {name}";
                }, "T4"
            );

            t3.Start();
            return t3;
        }

        private static async Task<string> InitTask2_v2()
        {
            Task<string> t2 = new Task<string>(
                () =>
                {
                    DoSomeThing(8, "T2", ConsoleColor.Blue);
                    return "Return from t3";
                }
            );
            t2.Start();
            var kq = await t2;
            System.Console.WriteLine("T2 done");
            // return t2.Result;
            return kq;
        }

        private static async Task<string> InitTask3_v2()
        {
            Task<string> t3 = new Task<string>(
               (object obj) =>
               {
                   string name = obj as string;
                   DoSomeThing(5, name, ConsoleColor.Blue);
                   return $"Return from {name}";
               }, "T4"
           );

            t3.Start();
            var kq = await t3;
            System.Console.WriteLine("T3 done");
            // Do đây là Task<string> và là phương thức bất đồng bộ
            // nên phải trả về 1 giá trị theo kiểu dữ liệu của Task

            // return t3.Result;
            return kq;
        }

        public static void Demo1()
        {
            Task<string> t2 = new Task<string>(
                () =>
                {
                    DoSomeThing(8, "T2", ConsoleColor.Blue);
                    return "Return from t3";
                }
            );

            Task<string> t3 = new Task<string>(
                (object obj) =>
                {
                    string name = obj as string;
                    DoSomeThing(5, name, ConsoleColor.Blue);
                    return $"Return from {name}";
                }, "T4"
            );

            t2.Start();
            t3.Start();
            DoSomeThing(5, "T1", ConsoleColor.Red); // chạy trên 1 thread
            Task.WaitAll();
            string s1 = t2.Result;
            string s2 = t3.Result;
            System.Console.WriteLine(s1 + " \n" + s2);
        }

        public static void Demo2()
        {
            Task<string> t2 = InitTask2();
            Task<string> t3 = InitTask3();
            DoSomeThing(5, "T1", ConsoleColor.Red); // chạy trên 1 thread
            Task.WaitAll(t2, t3);
            string s1 = t2.Result;
            string s2 = t3.Result;
            System.Console.WriteLine(s1 + " \n" + s2);
        }

        public static async Task Demo3()
        {
            Task<string> t2 = InitTask2_v2();
            Task<string> t3 = InitTask3_v2();
            DoSomeThing(5, "T1", ConsoleColor.Red); // chạy trên thread chính
            var kq2 = await t2;
            var kq3 = await t3;
            System.Console.WriteLine("Thread Id: " + Thread.CurrentThread.ManagedThreadId);
            System.Console.WriteLine(kq2 + " \n" + kq3);
        }

        public static async Task<string> GetContentWeb(string url)
        {
            HttpClient httpClient = new HttpClient();
            System.Console.WriteLine("Bat dau tai " + Thread.CurrentThread.ManagedThreadId);
            HttpResponseMessage kq = await httpClient.GetAsync(url);
            System.Console.WriteLine("Bat dau doc noi dung " + Thread.CurrentThread.ManagedThreadId);
            string content = await kq.Content.ReadAsStringAsync();
            System.Console.WriteLine("Hoan thanh " + Thread.CurrentThread.ManagedThreadId);
            return content;
        }

        public static async Task Demo4()
        {
            // Nếu mà đưa DoSomeThing lên trước thì mất đi tính bất đồng bộ vì nó phải đợi thread chính thực hiện DoSomeThing
            // xong thì thread chính mới tạo ra 1 thread bất đồng bộ để cho task chạy. Lúc này thì chạy lần lượt như đơn luồng rồi
            var task = GetContentWeb("https://xuanthulab.net");
            System.Console.WriteLine("lam gi do " + Thread.CurrentThread.ManagedThreadId);
            DoSomeThing(5, "T1", ConsoleColor.Red); // chạy trên 1 thread
            var content = await task;
            System.Console.WriteLine($"id: {Thread.CurrentThread.ManagedThreadId}");
            System.Console.WriteLine(content);
        }
    }
}