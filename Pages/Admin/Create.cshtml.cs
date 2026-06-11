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
            UserPage._blocks.Add(new UserPageBlock());
            if (UserPage.userId == null)
            {
                UserPage.userId = _context.UserPage.Count() + 1;
                UserPage.GetBlock(0).UserPageId = UserPage.userId;
                List<UserPageBlock> _blocks = await _context.UserBlockPage.ToListAsync();
                int _id = 0;
                foreach (var item in _blocks)
                {
                    if (item.Id >= _id)
                    {
                        _id = item.Id + 1;
                    }
                }
                UserPage.GetBlock(0).Id = _id;
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }


            _context.UserPage.Add(UserPage);

            _context.UserBlockPage.AddRange(UserPage._blocks);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
