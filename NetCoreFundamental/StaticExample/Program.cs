namespace StaticExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Static Example");
            DatabaseConnection.Instance.Testing();

            // Một object không thể truy cập vào 1 static method
            // DatabaseConnection.Instance.Testing2();

            // Ta chỉ có thể truy cập vào static method thông qua tên của class
            DatabaseConnection.Testing2();
        }
    }
}