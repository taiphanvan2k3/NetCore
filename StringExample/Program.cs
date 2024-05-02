using StringExample.Models;

namespace StringExample
{
    public class Program
    {
        public static void Example1()
        {
            string str1 = null;
            string str2 = "abc";
            string str3 = str1 + str2;

            // Không quăng ra ngoại lệ
            System.Console.WriteLine($"str3: {str3}");
            System.Console.WriteLine($"Check: {str1 == str2}");

            // Bị quăng ngoại lệ nếu truy cập property
            // System.Console.WriteLine($"Length: {str1.Length}");

            string str4 = "abc";
            string str5 = @"Xin chào
            các bạn.";
            System.Console.WriteLine(str4);
            System.Console.WriteLine(str5);
        }

        public static void Example2()
        {
            Dictionary<KeyValuePair<int, string>, int> d = new();
            d.Add(new KeyValuePair<int, string>(1, "1,2"), 2);
            d.Add(new KeyValuePair<int, string>(1, "1,3"), 20);
            if (d.ContainsKey(new KeyValuePair<int, string>(1, "1,3")))
            {
                Console.WriteLine("yes");
            }
            else
            {
                System.Console.WriteLine("no");
            }
        }

        public static void Test(string name)
        {
            ////System.Console.WriteLine(name);
            
        }

        static void Main(string[] args)
        {
            // Example1();
            // Test(name: "anc");
            // Example2();
            // DocxExample.Example2();
            new ExportDocxModel().ExportToDocx();
        }
    }
}