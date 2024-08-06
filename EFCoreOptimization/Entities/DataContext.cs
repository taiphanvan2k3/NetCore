using Microsoft.EntityFrameworkCore;

namespace EFCoreOptimization.Entities
{
    public class DataContext(DbContextOptions<DataContext> options, QueryCountingInterceptor queryCountingInterceptor) : DbContext(options)
    {
        private readonly QueryCountingInterceptor _queryCountingInterceptor = queryCountingInterceptor;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.AddInterceptors(_queryCountingInterceptor);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(builder =>
            {
                builder.ToTable("Companies");
                builder.HasMany(c => c.Employees)
                    .WithOne(e => e.Company)
                    .HasForeignKey(e => e.CompanyId)
                    .IsRequired(); // CompanyId is required

                builder.HasData(new Company()
                {
                    Id = 1,
                    Name = "Awesome Company",
                });
            });

            modelBuilder.Entity<Employee>(builder =>
            {
                builder.ToTable("Employees");
                builder.Property(e => e.Salary)
                    .HasPrecision(18, 2); // Salary is decimal(18, 2)

                var employees = Enumerable
                    .Range(1, 1000)
                    .Select(id => new Employee()
                    {
                        Id = id,
                        Name = $"Employee {id}",
                        Salary = 100,
                        CompanyId = 1
                    })
                    .ToList();
                builder.HasData(employees);
            });
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}