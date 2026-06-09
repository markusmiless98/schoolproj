using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PublicSchoolProj.Models;

namespace PublicSchoolProj.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PublicSchoolProj.Models.UserPage> UserPage { get; set; } = default!;

        public DbSet<PublicSchoolProj.Models.UserPageBlock> UserBlockPage { get; set; } = default!;
    }
}
