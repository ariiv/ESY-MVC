using ESY_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ESY_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AuditController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetAuditData")]
        public IActionResult GetAuditLogs(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _dbContext.Audits.AsQueryable();

            if (fromDate != null)
            {
                query = query.Where(a => DateTime.ParseExact(a.TimeStamp, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) >= fromDate);
            }

            if (toDate != null)
            {
                query = query.Where(a => DateTime.ParseExact(a.TimeStamp, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) <= toDate);
            }

            var auditLogs = query.ToList();
            auditLogs = auditLogs.OrderByDescending(a => DateTime.ParseExact(a.TimeStamp, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)).Take(10).ToList();


            var jsonResult = new JsonResult(auditLogs, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            return jsonResult;
        }
    }
}
