using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class IndexModel : PageModel
    {
        private readonly UserPageController _controlPage;

        public IndexModel(IUserPageController context)
        {
            _controlPage = (UserPageController)context;
        }

        public IList<UserPage> UserPage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            UserPage = await _controlPage.ReadAll();
        }
    }
}
