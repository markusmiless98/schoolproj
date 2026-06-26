using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    LayoutCSS _temp = new LayoutCSS();
                    _temp.SetDefault();
                    _temp.Id = 1;

                    _context.Attach(_temp).State = EntityState.Added;

                    try
                    {
                        await _context.SaveChangesAsync();
                        id = _temp.Id;
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw new Exception("Failed to get a new layout");
                    }
                }
                id = _id.Id;
            }

            var layoutcss = await _context.LayoutCSS.FirstOrDefaultAsync(m => m.Id == id);
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
            LayoutCSS _temp = await _context.LayoutCSS.FirstOrDefaultAsync(m => m.Id == 1);

            if (_temp == null)
            {
                throw new Exception("Failed to find the CSS Layout");
            }


            _temp.OverWrite(LayoutCSS);

            _context.Attach(_temp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                CSSHandler temp = new CSSHandler();

                await temp.WriteTo(_temp.ConvertIntoString());
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

            return Page();
            //return RedirectToPage("./Index");
        }

        private bool LayoutCSSExists(int id)
        {
            return _context.LayoutCSS.Any(e => e.Id == id);
        }
    }
}
