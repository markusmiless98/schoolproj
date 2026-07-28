using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PublicDatabaseAPI.Controllers;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;

namespace PublicSchoolProj.Pages.Admin
{
    public class DetailsModel : PageModel
    {
        private readonly UserPageController _context;

        public DetailsModel(IUserPageController context)
        {
            _context = (UserPageController) context;
        }

        public UserPage UserPage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Admin/Index");
            }

            var userpage = await _context.Read((int)id);
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
    }
}
