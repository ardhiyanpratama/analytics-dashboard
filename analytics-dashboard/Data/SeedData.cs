using analytics_dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace analytics_dashboard.Data
{
    public class SeedData
    {
        public static void Seed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var paymentContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            paymentContext.Database.Migrate();

        }
    }
}
