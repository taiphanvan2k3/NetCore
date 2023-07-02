namespace SanPham
{
    public partial class Product
    {
        // Chia nhỏ code ra
        public string ShowInfo()
        {
            return $"{this.Name}, {this.price}";
        }
    }
}