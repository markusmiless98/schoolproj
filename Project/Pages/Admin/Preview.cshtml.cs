using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicDatabaseAPI.Controllers;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicSchoolProj.Pages.Admin
{
    [Authorize]
    public class PreviewModel : PageModel
    {
        private readonly UserPageController _controlPage;
        private readonly UserPageBlockController _controlBlock;

        public PreviewModel(IUserPageController _context, IUserPageBlockController _contextBlock)
        {
            _controlPage = (UserPageController)_context;
            _controlBlock = (UserPageBlockController)_contextBlock;
        }

        public UserPage UserPage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userpage = await _controlPage.Read((int)id);
            if (userpage == null)
            {
                return NotFound();
            }
            UserPage = userpage;

            return Page();
        }

    }
}
