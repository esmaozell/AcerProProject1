using AcerProProject1.Models;
using Microsoft.EntityFrameworkCore;

namespace AcerProProject1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<LocalUser> LocalUsers { get; set; }

        public DbSet<TargetAPI> TargetApis { get; set; }
    }
}
