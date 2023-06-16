namespace MyLib
{
    public static class StaticClass
    {
        // Muốn mở rộng phương thức cho lớp nào thì tham số đầu tiên
        // phương thức phải là đối tượng của lớp đó và có từ khóa this
        public static void Print(this string str, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            System.Console.WriteLine(str);
        }

        public static double BinhPhuong(this double x)
        {
            return x * x;
        }

        public static double Sqrt(this double x)
        {
            return Math.Sqrt(x);
        }
    }
}