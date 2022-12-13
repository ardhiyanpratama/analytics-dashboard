namespace analytics_dashboard.Core.IRepositories
{
    public interface IAnalyticsRepository
    {
        Task<float> CountUserClick();
    }
}
