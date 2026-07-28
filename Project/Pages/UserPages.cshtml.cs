using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PublicDatabaseAPI.Controllers;
using PublicDatabaseAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicSchoolProj.Pages
{
    public class UserPagesModel : PageModel
    {
        private readonly UserPageController _context;
        private readonly UserPageBlockController _blockContext;

        public UserPagesModel(IUserPageController context, IUserPageBlockController blockContext)
        {
            if (context == null)
            {
                throw new NullReferenceException("Lack of UserPageController: " + context);
            }
            if (blockContext == null)
            {
                throw new NullReferenceException("Lack of UserPageBlockController: " + blockContext);
            }
            _context = (UserPageController)context;
            _blockContext = (UserPageBlockController)blockContext;
        }

        public IList<UserPage> UserPages { get; set; } = default!;

        public int _focus = -1;

        public async Task OnGetAsync(int id = -1)
        {
            UserPages = await _context.ReadAll();
            if (_focus != id)
            {
                if (id < 0)
                {
                    return;
                }
                UserPage _page = await _context.Read(id);
                if (_page == null)
                {
                    _focus = -1;
                }
                else
                {
                    // Adjust later to be more API-friendly
                    _focus = 0;
                    foreach (var item in UserPages)
                    {
                        if (item == _page)
                        {
                            item._blocks = await _blockContext.ReadAll(id);
                            item.views++;
                            await _context.Update(item);
                            break;
                        }
                        _focus++;
                    }
                }
            }
        }
    }
}
