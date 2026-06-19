using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PublicSchoolProj.Data;
using PublicSchoolProj.Models;

namespace PublicSchoolProj.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly PublicSchoolProj.Data.ApplicationDbContext _context;

        public IndexModel(PublicSchoolProj.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<UserPage> UserPage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            UserPage = await _context.UserPage.ToListAsync();
        }
    }
}
