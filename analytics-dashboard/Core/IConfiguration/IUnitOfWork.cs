using analytics_dashboard.Core.IRepositories;

namespace analytics_dashboard.Core.IConfiguration
{
    public interface IUnitOfWork
    {
        IAnalyticsRepository Analytics { get; }

        Task CompleteAsync();
    }
}
