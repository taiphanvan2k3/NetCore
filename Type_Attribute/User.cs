using System.ComponentModel.DataAnnotations;
using MyAttribute;

namespace Type_Attribute
{
    [Mota("Lop chua thong tin ve mot user tren he thong")]
    public class User
    {
        [Mota("Ten cua user")]
        [Required(ErrorMessage = "Name phai thiet lap")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Ten phai dai tu 3 den 100 ki tu")]
        public string Name { get; set; } = null!;

        [Mota("Du lieu tuoi")]
        [Range(18, 80, ErrorMessage = "Tuoi phai tu 18-80")]
        public int Age { get; set; }

        [Mota("So dien thoai ca nhan")]
        // [Phone]
        [RegularExpression(@"^(?!0+$)((\+\d{1,2}[- ]?)|0)[35789](?!0+$)\d{8}$", ErrorMessage = "So dien thoai khong hop le")]
        
        public string? PhoneNumber { get; set; }

        [Mota("Email ca nhan")]
        [EmailAddress(ErrorMessage = "Dia chi email khong hop le")]
        public string? Email { get; set; }

        // Đánh dấu 1 phương thức, thuộc tính, class bị lỗi thời
        [Obsolete("Phương thức này không nên sử dụng nữa. Sử dụng ShowInfo để thay thế")]
        public void PrintInfo()
        {
            System.Console.WriteLine("Name: " + Name);
        }
    }
}