namespace App
{
    public class Program
    {
        private static void Test1()
        {
            Student s = new Student()
            {
                Id = 1,
                Name = "Nguyễn Văn A"
            };

            Student s2 = new Student()
            {
                Id = 1,
                Name = "Nguyễn Văn B"
            };

            System.Console.WriteLine("So sanh ==: " + (s == s2));
            System.Console.WriteLine("So sanh equals:" + s.Equals(s2));
        }

        private static void Test2()
        {
            List<Student> li = new List<Student>()
            {
                new Student()
                {
                    Id = 1,
                    Name = "Nguyễn Văn A"
                },

                new Student()
                {
                    Id = 2,
                    Name = "Nguyễn Văn B"
                }
            };

            int idx = li.IndexOf(new Student() { Id = 2 });

            // idx = 1
            System.Console.WriteLine(idx);
        }

        private static string Test3(Student s)
        {
            if(String.IsNullOrEmpty(s.Name))
            {
                // Dùng nameof chứ không dùng tên cụ thể, vì giả sử sau này Name đổi thành MyName thì phải sửa lại
                // throw new Exception($"Loi xay ra tai thuoc tinh: MyName");
                throw new Exception($"Loi xay ra tai thuoc tinh: {nameof(s.Name)}");
            }
            return "Kq: " + s.Name;
        }

        static void Main(string[] args)
        {
            Student s = new Student() { Id = 1 };
            try
            {
                string tmp = Test3(s);
                System.Console.WriteLine(tmp);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}