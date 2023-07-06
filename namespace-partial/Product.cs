using System.ComponentModel.DataAnnotations;

namespace SanPham
{
    public partial class Product
    {
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public decimal price { get; set; }
    }
}