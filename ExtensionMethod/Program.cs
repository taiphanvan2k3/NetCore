using MyLib;

namespace App
{
    // 
    public class Program
    {
        private static void Vidu1()
        {
            int[] mang = { 1, 2, 3, 4 };
            // System.Linq đã mở rộng chức năng của lớp Array
            int max = mang.Max();
            System.Console.WriteLine(max);
        }

        private void Vidu2()
        {
            string s = "Xin chào các bạn";
            // Mong muốn s có phương thức Print s.Print(Color...);
            s.Print(ConsoleColor.Yellow);
            s.Print(ConsoleColor.DarkCyan);

            System.Console.WriteLine((4.0).BinhPhuong());
            System.Console.WriteLine((4.0).Sqrt());
        }

        static void Main(string[] args)
        {
            // System.Console.WriteLine(new TestClass("Hello").Name);
            Vector v1 = new Vector(1, 2);
            Vector v2 = new Vector(2, 3);
            (v1 + v2).ShowInfo();
            (v1 + 10).ShowInfo();
            v1[0] = 100;
            System.Console.WriteLine("X:" + v1[0]);
            System.Console.WriteLine("y:" + v1["y"]);
        }
    }
}