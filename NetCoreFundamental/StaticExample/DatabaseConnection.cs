namespace StaticExample
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _Instance = null;
        private static readonly object _lock = new();
        private readonly int x = 100;

        public static DatabaseConnection Instance
        {
            get
            {
                lock (_lock)
                {
                    // Ta dùng lock để đảm bảo rằng chỉ có một thread được phép truy cập vào critical section tại 1 thời điểm
                    _Instance ??= new();
                    FakeConnect();
                }

                // Đoạn code nằm sau khối lock này chỉ được thực thi khi thread hiện tại đã thực thi xong critical section
                return _Instance;
            }
        }

        private static void FakeConnect()
        {
            Console.WriteLine("Connected to database");
        }

        public void Testing()
        {
            Console.WriteLine("Testing" + x);
        }

        public static void Testing2()
        {
            Console.WriteLine("Testing2");
        }
    }
}