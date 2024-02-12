namespace NetCore.CustomExceptions
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                throw new CustomException(404, "This cadre is not exist");
            }
            catch (CustomException e)
            {
                System.Console.WriteLine(e.ErrorCode);
                System.Console.WriteLine(e.Message);
            }
        }
    }
}