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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserPageController _controlPage;
        private readonly UserPageBlockController _controlBlock;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
            _controlPage = context.GetUserPageController();
            _controlBlock = context.GetUserPageBlockController();
        }

        [BindProperty]
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

            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostDeleteAllAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userpages = await _context.UserPage.ToListAsync();
            var userblock_pages = await _context.UserBlockPage.ToListAsync();
            if (userpages != null)
            {
                await _controlBlock.HandleFromList(EntityState.Deleted, userblock_pages);

                await _controlPage.HandleFromList(EntityState.Deleted, userpages);
            }

            return RedirectToPage("./Index");
        }
    }
}
