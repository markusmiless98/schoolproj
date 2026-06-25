using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicCssAPI.Handler;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;

namespace PublicSchoolProj.Pages.Admin
{
    public class CSSEditModel : PageModel
    {
        private readonly PublicDatabaseAPI.Data.ApplicationDbContext _context;

        public CSSEditModel(PublicDatabaseAPI.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LayoutCSS LayoutCSS { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            if (id == null)
            {
                //return NotFound();
                var _id = await _context.LayoutCSS.FirstOrDefaultAsync();
                if (_id == null)
                {
                    return NotFound();
                }
                id = _id.Id;
            }

            var layoutcss =  await _context.LayoutCSS.FirstOrDefaultAsync(m => m.Id == id);
            if (layoutcss == null)
            {
                return NotFound();
            }
            LayoutCSS = layoutcss;
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

            _context.Attach(LayoutCSS).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                CSSHandler temp = new CSSHandler();

                await temp.WriteTo(LayoutCSS.ConvertIntoString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LayoutCSSExists(LayoutCSS.Id))
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

        private bool LayoutCSSExists(int id)
        {
            return _context.LayoutCSS.Any(e => e.Id == id);
        }
    }
}
