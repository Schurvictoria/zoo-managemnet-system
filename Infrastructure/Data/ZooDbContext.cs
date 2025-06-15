using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ZooDbContext : DbContext
    {
        public ZooDbContext(DbContextOptions<ZooDbContext> options) : base(options)
        {
        }
    }
}



