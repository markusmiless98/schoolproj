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
    public class DeleteModel : PageModel
    {
        private readonly PublicSchoolProj.Data.ApplicationDbContext _context;

        public DeleteModel(PublicSchoolProj.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
            else
            {
                UserPage = userpage;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userpage = await _context.UserPage.FindAsync(id);
            if (userpage != null)
            {
                UserPage = userpage;
                _context.UserPage.Remove(UserPage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
