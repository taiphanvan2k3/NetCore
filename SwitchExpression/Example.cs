using System.ComponentModel.DataAnnotations;
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

            Console.WriteLine(res1 + " " + res2);
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
            Console.WriteLine(result + " " + result2);
        }


        public static void Example4()
        {
            Student s1 = new()
            {
                Name = "Nguyen Van Annh",
            };

            Student s2 = new()
            {
                Name = "Nguyen Thi B"
            };

            // Nếu sử dụng điều kiện là: var (a,b) thì điều kiện này luôn đúng vì (s1.Name, s2.Name) thì luôn thoả
            // mãn var (a,b)
            var (a1, a2) = (s1.Name, s2.Name) switch
            {
                _ when s1.Name.Length > s2.Name.Length => ("2", "1"),
                _ => ("0", "0")
            };

            var res = s1.Name.Length switch
            {
                > 10 => "Chiều dài lớn hơn 10",
                _ => "Chiều dài không vượt quá 10"
            };

            var res2 = s1 switch
            {
                { Name.Length: var x } when x > 30 => "Chiều dài tên lớn hơn 10",
                _ when s1.Name.EndsWith("Anh") => "Tên có chứa chữ Anh",
                _ => "Trường hợp ngược lại"
            };
            Console.WriteLine(a1 + " " + a2);
            Console.WriteLine(res);
            Console.WriteLine(res2);
        }

        public readonly struct Point
        {
            public Point(int x, int y) => (X, Y) = (x, y);

            public int X { get; }
            public int Y { get; }
        }

        static Point Transform(Point point) => point switch
        {
            { X: 0, Y: 0 } => new Point(0, 0),
            { X: var x, Y: var y } when x < y => new Point(x + y, y),
            { X: var x, Y: var y } when x > y => new Point(x - y, y),
            { X: var x, Y: var y } => new Point(2 * x, 2 * y),
        };
    }
}