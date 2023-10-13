using NetCore.SwitchExpressione;

namespace NetCore.SwitchExpression
{
    public class Example
    {
        public static void Example1()
        {
            var gender = false;
            Student s = gender switch
            {
                true => new Student()
                {
                    Name = "Nguyen Van A",
                    Gender = "Nam"
                },

                // default và nó yêu cầu được đặt ở cuối
                _ => new Student()
                {
                    Name = "Nguyen Thi B",
                    Gender = "Nu"
                }
            };
            Console.WriteLine(s.Name + "," + s.Gender);
        }

        public static void Example2()
        {
            Student s1 = new()
            {
                Name = "Nguyen Van Anh",
            };

            Student s2 = new()
            {
                Name = "Nguyen Thi B"
            };

            var (res1, res2) = (s1.Name, s2.Name) switch
            {
                var (a, b) when a.Length > b.Length => ("2", "1"),
                var (a, b) when a.Length < b.Length => ("1", "2"),
                var (_, _) => ("0", "0")
            };
            System.Console.WriteLine(res1 + " " + res2);
        }

        public static void Example3()
        {
            Student s1 = new()
            {
                Name = "Nguyen Van Anh",
                Gender = "Nam"
            };

            s1 = null;
            int result = s1 switch
            {
                { Gender: var gender } when gender == "Nam" => 1,
                { Name: var name, Gender: var gender } when !string.IsNullOrEmpty(name) && gender == "Nu" => 0,
                _ => -1
            };

            int result2 = s1?.Gender switch
            {
                "Nam" => 1,
                "Nu" => 0,
                _ => -1
            };

            int result3 = s1?.Age ?? 0;
            System.Console.WriteLine(result + " " + result2);
        }
    }
}