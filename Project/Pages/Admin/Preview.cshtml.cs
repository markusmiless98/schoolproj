using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicDatabaseAPI.Controllers;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;

namespace PublicSchoolProj.Pages.Admin
{
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
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Admin/Index");
            }

            var userpage = await _controlPage.Read((int)id);
            if (userpage == null)
            {
                return NotFound();
            }
            UserPage = userpage;

            //await SetUpLinks();

            return Page();
        }

        // API handles it now
        /*
        private async Task SetUpLinks()
        {
            if (UserPage == null) return;
            if (UserPage.links == null) return;

            if (UserPage.links.Count > 0)
            {
                List<Links> _links = new List<Links>();
                foreach (var item in UserPage.GetLinks())
                {
                    
                    var _linkedPage = await _controlPage.Read(item);
                    if (_linkedPage != null)
                    {
                        _links.Add(new Links(_linkedPage.title, GetLink(item.ToString())));
                    }
                }
                UserPage._listOfLinks = _links;
            }
        }
        */
        private string GetLink(string _id)
        {
            if (_id == null) return "";
            return "Admin/Preview?id=" + _id;
        }
    }
}
