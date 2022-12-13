using analytics_dashboard.Core.IConfiguration;
using analytics_dashboard.Core.IRepositories;
using analytics_dashboard.Core.Repositories;
using analytics_dashboard.Models;
using Microsoft.AspNetCore.Identity;

namespace analytics_dashboard.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationContext _context;
        private readonly ILogger _logger;

        public IAnalyticsRepository Analytics { get; private set; }

        public UnitOfWork(ApplicationContext context, ILoggerFactory logger)
        {
            _context = context;
            _logger = logger.CreateLogger("logs");

            Analytics = new AnalyticsRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
