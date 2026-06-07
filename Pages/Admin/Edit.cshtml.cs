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
    public class EditModel : PageModel
    {
        private readonly PublicSchoolProj.Data.ApplicationDbContext _context;

        public EditModel(PublicSchoolProj.Data.ApplicationDbContext context)
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

            var userpage =  await _context.UserPage.FirstOrDefaultAsync(m => m.Id == id);
            if (userpage == null)
            {
                return NotFound();
            }
            UserPage = userpage;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserPage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPageExists(UserPage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserPageExists(int id)
        {
            return _context.UserPage.Any(e => e.Id == id);
        }
    }
}
