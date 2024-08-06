using Dapper;
using EFCoreOptimization.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCoreOptimization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(DataContext context, QueryCountingInterceptor queryCountingInterceptor) : ControllerBase
    {
        private readonly DataContext _context = context;
        private readonly QueryCountingInterceptor _queryCountingInterceptor = queryCountingInterceptor;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.OrderBy(e => e.Id).Take(10).ToListAsync();
        }

        [HttpGet("increase-salaries")]
        public async Task<ActionResult> IncreaseSalaries([FromQuery] int companyId)
        {
            _queryCountingInterceptor.Reset(); // Reset the counter at the beginning
            var company = await _context.Companies
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                return NotFound();
            }

            foreach (var employee in company.Employees)
            {
                employee.Salary *= 1.1m;
            }
            company.LastSalaryUpdateUtc = DateTime.UtcNow;

            // Nếu có 1000 nhân viên thì sẽ có 1000 câu lệnh UPDATE cho Employee và 1 câu lệnh UPDATE cho Company
            await _context.SaveChangesAsync();

            var queryCount = _queryCountingInterceptor.QueryCount;
            return Ok(new { queryCount });
        }

        [HttpGet("increase-salaries-sql")]
        public async Task<ActionResult> IncreaseSalariesSQL([FromQuery] int companyId)
        {
            _queryCountingInterceptor.Reset(); // Reset the counter at the beginning
            var company = await _context.Companies
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                return NotFound();
            }

            await _context.Database.BeginTransactionAsync();
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE Employees SET Salary = Salary * 1.1 WHERE CompanyId = {companyId}");

            
            company.LastSalaryUpdateUtc = DateTime.UtcNow;

            // Nếu có 1000 nhân viên thì sẽ có 1 câu lệnh UPDATE cho Employee và 1 câu lệnh UPDATE cho Company
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();

            var queryCount = _queryCountingInterceptor.QueryCount;
            return Ok(new { queryCount });
        }

        [HttpGet("increase-salaries-dapper")]
        public async Task<ActionResult> IncreaseSalariesDapper([FromQuery] int companyId)
        {
            _queryCountingInterceptor.Reset(); // Reset the counter at the beginning
            var company = await _context.Companies
                .Include(c => c.Employees)
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null)
            {
                return NotFound();
            }

            var transaction = await _context.Database.BeginTransactionAsync();

            // Sử dụng Dapper để thực hiện câu lệnh UPDATE
            // Vì Dapper không chạy trên cùng 1 transaction với EF Core nên cần truyền transaction vào để nói cho Dapper chạy trên transaction đó
            await _context.Database.GetDbConnection().ExecuteAsync(
                "UPDATE Employees SET Salary = Salary * 1.1 WHERE CompanyId = @CompanyId",
                new { CompanyId = companyId },
                transaction.GetDbTransaction());

            company.LastSalaryUpdateUtc = DateTime.UtcNow;

            // Nếu có 1000 nhân viên thì sẽ có 1 câu lệnh UPDATE cho Employee và 1 câu lệnh UPDATE cho Company
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();

            var queryCount = _queryCountingInterceptor.QueryCount;
            return Ok(new { queryCount });
        }
    }
}