using analytics_dashboard.Core.IConfiguration;
using library.Adapter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace analytics_dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerAdapter<AnalyticsController> _logger;
        public AnalyticsController(IUnitOfWork unitOfWork, ILogger<AnalyticsController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = new LoggerAdapter<AnalyticsController>(logger);
        }

        [HttpGet("count-click")]
        [AllowAnonymous]
        public async Task<IActionResult> CountClick()
        {
            var result = await _unitOfWork.Analytics.CountUserClick();

            return Ok(result);
        }
    }
}
