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
using System.Net.Http;
using System.Threading.Tasks;

namespace PublicSchoolProj.Pages.Admin
{
    public class CreateModel : PageModel
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserPageController _pageControl;

        public CreateModel(IUserPageController context)
        {
            //_context = context;
            
            _pageControl = (UserPageController)context;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Admin/Index");
            }
            return Page();
        }

        [BindProperty]
        public UserPage UserPage { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            UserPage.Id = await GetNewId();
            if (UserPage.userId == null)
            {
                UserPage.userId = UserPage.Id;
            }

            await _pageControl.Create(UserPage);

            // TO BE DONE; Deserialize from JSON the UserPage then send it using 'await client. PostAsync ( " api /Products/" , httpContent );'

            //await _pageControl.Create(UserPage);


            return Page(); // To check i guess
            //return RedirectToPage("./Index");
        }

        public virtual async Task<int> GetNewId()
        {
            var _pages = await _pageControl.ReadAll();

            if (_pages != null)
            {
                int i = 0;

                // Later just check for highest id with above search
                foreach (var item in _pages)
                {
                    if (item.Id == i)
                    {
                        i = item.Id + 1;
                    }
                }

                return i;
            }


            return 1;
        }
    }
}
