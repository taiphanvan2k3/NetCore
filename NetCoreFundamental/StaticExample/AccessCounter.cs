namespace StaticExample
{
    public class AccessCounter
    {
        private int _Counter = 0;
        private static AccessCounter _Instance = new();

        private static AccessCounter Instance()
        {
            return _Instance;
        }

        public int Counter
        {
            get; set;
        }
    }
}