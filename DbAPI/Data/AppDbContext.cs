using Microsoft.EntityFrameworkCore;

namespace DbAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ServiceObject> Objects { get; set; }
        public DbSet<ServiceBooking> Bookings { get; set; }
    }
}
