using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicSchoolProj.Data;
using PublicSchoolProj.Models;

namespace PublicSchoolProj.Pages.Admin
{
    public class PreviewModel : PageModel
    {
        private readonly PublicSchoolProj.Data.ApplicationDbContext _context;

        public PreviewModel(PublicSchoolProj.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public UserPage UserPage { get; set; } = default!;



        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userpage = await _context.UserPage.FirstOrDefaultAsync(m => m.Id == id);
            if (userpage == null)
            {
                return NotFound();
            }
            UserPage = userpage;
            return Page();
        }
    }
}
