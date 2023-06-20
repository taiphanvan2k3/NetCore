namespace MyAttribute
{
    // Mô tả attribute này được sử dụng ở đâu
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class)]
    // Phải đặt tên class Attribute có hậu tố Attribute
    public class MotaAttribute : Attribute
    {
        public string ThongTinChiTiet { get; set; }
        public MotaAttribute(string _ThongTinchiTiet)
        {
            ThongTinChiTiet = _ThongTinchiTiet;
        }


    }
}