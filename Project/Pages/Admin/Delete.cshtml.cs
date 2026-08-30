using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PublicDatabaseAPI.Controllers;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;
using PublicDatabaseAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicSchoolProj.Pages.Admin
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly UserPageController _controlPage;
        private readonly UserPageBlockController _controlBlock;
        private readonly UserPageService _pageService;

        //public DeleteModel(IUserPageController _page, IUserPageBlockController _block)
        public DeleteModel(IUserPageService pageService)
        {
            //_controlPage = (UserPageController) _page;
            //_controlBlock = (UserPageBlockController) _block;
            _pageService = (UserPageService)pageService;
        }

        [BindProperty]
        public UserPage UserPage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userpage = await _pageService.GetPageAsync((int)id);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*
            var userpage = await _controlPage.Read((int)id);
            if (userpage != null)
            {
                UserPage = userpage;


                // Clean up later
                List<UserPageBlock> _tbd = new List<UserPageBlock>();
                foreach (var item in await _controlBlock.ReadAll())
                {
                    if (item.UserPageId == UserPage.Id)
                    {
                        _tbd.Add(item);
                    }
                }

                await _controlBlock.HandleFromList(EntityState.Deleted, _tbd);

                await _controlPage.Delete(UserPage.Id);
            }
            */
            await _pageService.DeletePageAsync((int)id);
            
            return RedirectToPage("./Index");
        }
        // Unused
        public async Task<IActionResult> OnPostDeleteAllAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userpages = await _controlPage.ReadAll();
            var userblock_pages = await _controlBlock.ReadAll();
            if (userpages != null)
            {
                await _controlBlock.HandleFromList(EntityState.Deleted, userblock_pages);

                await _controlPage.HandleFromList(EntityState.Deleted, userpages);
            }

            return RedirectToPage("./Index");
        }
    }
}
