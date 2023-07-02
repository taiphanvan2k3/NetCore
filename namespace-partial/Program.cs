using MyNameSpace;
using XYZ = MyNameSpace.SubNameSpace;
using static System.Math;
using SanPham;

namespace AppConsole
{
    public class Program
    {
        static void Example1()
        {
            Class1.XinChao();
            XYZ.Class2.XinChao();

            // Do đã sử dụng using static System.Math nên không cần Math.Sqrt(4) nữa
            System.Console.WriteLine(Sqrt(4));
        }

        static void Main(string[] args)
        {
            Product product = new Product()
            {
                Name = "Laptop",
                price = 10000
            };
            System.Console.WriteLine(product.ShowInfo());;
        }
    }
}