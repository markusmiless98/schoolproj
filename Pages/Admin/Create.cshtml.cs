using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicSchoolProj.Data;
using PublicSchoolProj.Models;

namespace PublicSchoolProj.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly PublicSchoolProj.Data.ApplicationDbContext _context;

        public CreateModel(PublicSchoolProj.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserPage UserPage { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.UserPage.Add(UserPage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
