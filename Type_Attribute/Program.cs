using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MyAttribute;

namespace Type_Attribute
{
    public class Program
    {
        private static void Ex1()
        {
            string a = "a";
            Type t = a.GetType();
            System.Console.WriteLine(t.FullName);
            System.Console.WriteLine("Các thuộc tính:");
            t.GetProperties().ToList().ForEach(
              (PropertyInfo o) =>
              {
                  System.Console.WriteLine(o.Name);
              }
            );

            System.Console.WriteLine("Danh sach cac Files");
            t.GetFields().ToList().ForEach(
                (FieldInfo f) =>
                {
                    System.Console.WriteLine(f.Name);
                }
            );

            System.Console.WriteLine("Cac phuong thuc");
            t.GetMethods().ToList().ForEach(
               (MethodInfo f) =>
               {
                   System.Console.WriteLine(f.Name);
               }
           );
        }

        private static void Ex2()
        {
            User u = new User()
            {
                Name = "Tai",
                Age = 20,
                PhoneNumber = "0905",
                Email = "taiphanvan2403@gmail.com"
            };

            var properties = u.GetType().GetProperties().ToList();
            foreach (PropertyInfo property in properties)
            {
                string name = property.Name;
                // Lấy giá trị của thuộc tính đó.
                var value = property.GetValue(u);
                System.Console.WriteLine(name + ": " + value);
            }

            // u.PrintInfo();
            foreach (PropertyInfo property in properties)
            {
                // Mỗi property có thể có các attribute
                foreach (var attr in property.GetCustomAttributes(false))
                {
                    MotaAttribute? mota = attr as MotaAttribute;
                    if (mota != null)
                    {
                        System.Console.WriteLine(property.Name + ": " + mota.ThongTinChiTiet);
                    }
                }
            }
        }

        private static void Ex3()
        {
            User u = new User()
            {
                Name = "Tai",
                Age = 20,
                PhoneNumber = "0905...",
                Email = "taiphanvan2403"
            };

            ValidationContext context = new ValidationContext(u);
            var result = new List<ValidationResult>();

            // true: kiểm tra tất cả thuộc tính
            bool kq = Validator.TryValidateObject(u, context, result, true);
            if (!kq)
            {
                foreach (ValidationResult error in result)
                {
                    System.Console.WriteLine(error.MemberNames.FirstOrDefault() + ": " + error.ErrorMessage);
                }
            }
        }


        static void Main(string[] args)
        {
            Ex3();
        }
    }
}
