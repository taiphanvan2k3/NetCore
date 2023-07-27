using CodeUtils.ReadDescriptionFromEnum;

namespace CodeUtils
{
    public class Program
    {
        private static void Example1()
        {
            List<string> list = EnumExtension.GetEnumDescriptions<Status>();
        }

        static void Main(string[] args)
        {
            Example1();
        }
    }
}