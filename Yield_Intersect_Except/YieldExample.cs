namespace Yield_Intersect_Except
{
    public class YieldExample
    {
        public static IEnumerable<int> GetEvenNumbers(int max)
        {
            for (int i = 1; i <= max; i++)
            {
                if (i % 2 == 0)
                {
                    yield return i;
                }
            }
        }

        public static void Example1()
        {
            foreach(int number in GetEvenNumbers(10))
            {
                Console.WriteLine(number);
            }
        }
    }
}