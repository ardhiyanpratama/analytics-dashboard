using analytics_dashboard.Core.IRepositories;
using analytics_dashboard.Models;

namespace analytics_dashboard.Core.Repositories
{
    public class AnalyticsRepository:GenericRepository<Analytics>,IAnalyticsRepository
    {
        public AnalyticsRepository(ApplicationContext context, ILogger logger) : base(context, logger)
        {

        }

        public async Task<float> CountUserClick()
        {
            return dbSet.Where(x => x.TimeStamp.Value.Day == DateTime.UtcNow.Day).Count();
        }
    }
}
