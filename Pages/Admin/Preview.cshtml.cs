using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicSchoolProj.Classes;
using PublicSchoolProj.Data;
using PublicSchoolProj.Models;

namespace PublicSchoolProj.Pages.Admin
{
    public class PreviewModel : PageModel
    {
        private readonly PublicSchoolProj.Data.ApplicationDbContext _context;

        public PreviewModel(PublicSchoolProj.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
            UserPage = userpage;

            await SetUpLinks();

            return Page();
        }


        private async Task SetUpLinks()
        {
            if (UserPage == null) return;
            if (UserPage.links == null) return;

            if (UserPage.links.Count > 0)
            {
                List<Links> _links = new List<Links>();
                foreach (var item in UserPage.GetLinks())
                {

                    var _linkedPage = await _context.UserPage.FirstOrDefaultAsync(m => m.Id == item);
                    if (_linkedPage != null)
                    {
                        _links.Add(new Links(_linkedPage.title, GetLink(item.ToString())));
                    }
                }
                UserPage._listOfLinks = _links;
            }
        }
        private string GetLink(string _id)
        {
            if (_id == null) return "";
            return "Admin/Preview?id=" + _id;
        }
    }
}
