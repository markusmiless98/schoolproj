using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PublicDatabaseAPI.Models;

namespace PublicDatabaseAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PublicDatabaseAPI.Models.UserPage> UserPage { get; set; } = default!;

        public DbSet<PublicDatabaseAPI.Models.UserPageBlock> UserBlockPage { get; set; } = default!;
    }
}
