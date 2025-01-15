using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeatherForGardeners.Models;

namespace WeatherForGardeners.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
            
        }

        public DbSet<DayRecommendation> DayRecommendations { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }

    }
}
