//using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PublicDatabaseAPI.Controllers;
using PublicDatabaseAPI.Models;

namespace PublicDatabaseAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly UserPageController _UserPageController;
        private readonly UserPageBlockController _UserPageBlockController;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Debug.WriteLine("--Created: ApplicationDBContext");
            _UserPageController = new UserPageController(this);
            _UserPageBlockController = new UserPageBlockController(this);
        }
        public DbSet<PublicDatabaseAPI.Models.UserPage> UserPage { get; set; } = default!;

        public DbSet<PublicDatabaseAPI.Models.UserPageBlock> UserBlockPage { get; set; } = default!;

        public DbSet<PublicDatabaseAPI.Models.LayoutCSS> LayoutCSS { get; set; } = default!;


        public UserPageController GetUserPageController()
        {
            return _UserPageController;
        }
        public UserPageBlockController GetUserPageBlockController()
        {
            return _UserPageBlockController;
        }
    }
}
