using ESY_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.AccessControl;
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
        public async Task<IActionResult> GetAuditLogsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var auditLogs = _dbContext.Audits.ToList();

            if (fromDate != null)
            {
                auditLogs = auditLogs
                    .Where(a => a.TimeStamp != null && ParseTimeStampToDateTime(a.TimeStamp) >= fromDate.Value)
                    .ToList();
            }

            if (toDate != null)
            {
                auditLogs = auditLogs
                    .Where(a => a.TimeStamp != null && ParseTimeStampToDateTime(a.TimeStamp) <= toDate.Value)
                    .ToList();
            }

            auditLogs = auditLogs.OrderByDescending(a => ParseTimeStampToDateTime(a.TimeStamp)).Take(10).ToList();

            var jsonResult = new JsonResult(auditLogs, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            return jsonResult;
        }

        private static DateTime ParseTimeStampToDateTime (string timeStamp)
        {
            return DateTime.ParseExact(timeStamp, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
        }
    }
}
