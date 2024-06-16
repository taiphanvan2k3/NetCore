using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.RedisCaching.Models;
using RedisCaching.Data;
using RedisCaching.Models;
using RedisCaching.Service;

namespace RedisCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ILogger<DriversController> _logger;
        private readonly ICacheService _cacheService;
        private readonly AppDbContext _dbContext;

        public DriversController(AppDbContext dbContext, ICacheService cacheService, ILogger<DriversController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            DateTime startTime = DateTime.Now;
            var cacheData = _cacheService.GetData<List<Driver>>("drivers");
            if (cacheData == null)
            {
                cacheData = await _dbContext.Drivers.ToListAsync();
                _cacheService.SetData("drivers", cacheData, DateTimeOffset.Now.AddMinutes(5));
            }

            DateTime endTime = DateTime.Now;
            return Ok(new
            {
                cacheData,
                totalTime = $"{endTime.Subtract(startTime).TotalMilliseconds}ms"
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cacheData = _cacheService.GetData<Driver>($"drive{id}");
            if (cacheData == null)
            {
                cacheData = await _dbContext.Drivers.FindAsync(id);
                if (cacheData == null)
                {
                    return NotFound();
                }

                var expireTime = DateTimeOffset.Now.AddMinutes(5);
                _cacheService.SetData($"drive{id}", cacheData, expireTime);
            }

            return Ok(cacheData);
        }

        [HttpPost("add-driver")]
        public async Task<IActionResult> AddDriver(DriveDto driver)
        {
            var addedObj = await _dbContext.Drivers.AddAsync(new Driver
            {
                Name = driver.Name,
                DriverNumber = driver.DriverNumber
            });
            
            await _dbContext.SaveChangesAsync();
            var expireTime = DateTimeOffset.Now.AddMinutes(5);
            _cacheService.SetData($"drive{addedObj.Entity.Id}", addedObj.Entity, expireTime);

            // Xóa cache khi thêm mới driver để lúc get sẽ lấy dữ liệu mới nhất
            _cacheService.RemoveData("drivers");
            return Ok(addedObj.Entity);
        }

        [HttpDelete("delete-driver/{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var driver = await _dbContext.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _dbContext.Remove(driver);
            _cacheService.RemoveData($"drive{driver.Id}");
            _cacheService.RemoveData("drivers");
            await _dbContext.SaveChangesAsync();

            return Ok(driver);
        }
    }
}