namespace EFCoreOptimization.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public int CompanyId { get; set; }

        // null-forgiving operator (!) is used to indicate that the property will never be null
        public Company Company { get; set; } = null!;
    }
}