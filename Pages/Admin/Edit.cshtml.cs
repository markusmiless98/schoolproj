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
    public class EditModel : PageModel
    {
        private readonly PublicSchoolProj.Data.ApplicationDbContext _context;

        public EditModel(PublicSchoolProj.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserPage UserPage { get; set; } = default!;
        [BindProperty]
        public IList<UserPageBlock> UserPageBlocks { get; set; } = default!;
        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        int _id { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await GetUserPageById((int)id);
            if (UserPage == null)
            {
                return NotFound();
            }
            
            foreach (var item in UserPageBlocks)
            {
                if (!UserPage.GetBlocks().Contains(item))
                {
                    UserPage.GetBlocks().Add(item);
                }
            }

            if (id != null)
            {
                _id = (int)id;
            }

            return Page();
        }

        private async Task GetUserPageById(int id)
        {
            var userpage = await _context.UserPage.FirstOrDefaultAsync(m => m.Id == id);
            if (userpage == null)
            {
                throw new Exception("Failed to load the user page");
            }
            UserPage = userpage;

            UserPageBlocks = await _context.UserBlockPage.Where(e => e.UserPageId == id).ToListAsync();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserPage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPageExists(UserPage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Redirect(GetUserPage());
        }


        [BindProperty]
        public UserPageBlock UserPageBlock { get; set; } = default!;



        public async Task<IActionResult> OnPostBlockAsync(int id)
        {
            UserPageBlock _block = new UserPageBlock();
            _block.UserPageId = UserPage.userId;

            UserPage.Id = id;



            UserPage._blocks.Add(_block);
            _context.UserBlockPage.Add(_block);

            _context.Attach(UserPage).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Redirect(GetUserPage());
        }
        public async Task<IActionResult> OnPostDeleteBlockAsync(int id, int target)
        {
            var userpage = await _context.UserPage.FirstOrDefaultAsync(m => m.Id == id);
            if (userpage == null)
            {
                return NotFound();
            }
            UserPage = userpage;

            UserPage.GetBlocks().Clear();
            List<UserPageBlock> _blocks = new List<UserPageBlock>();
            _blocks = await _context.UserBlockPage.Where(m => m.UserPageId == id).ToListAsync();

            if (_blocks.Count < 1)
            {
                return NotFound();
            }

            UserPageBlock _block = UserPage.GetBlocks()[target];
            _block.UserPageId = UserPage.userId;

            UserPage.Id = id;
            UserPage._blocks.Remove(_block);

            _context.Attach(UserPage).State = EntityState.Modified;

            var userpageblock = await _context.UserBlockPage.FindAsync(_block.Id);
            if (userpageblock != null)
            {
                UserPageBlock = userpageblock;

                _context.UserBlockPage.Remove(userpageblock);

                await _context.SaveChangesAsync();
            }


            return Redirect(GetUserPage());
        }


        public async Task OnPostAddPicAsync(int id, int target)
        {
            if (UploadedImage == null)
            {
                throw new InvalidOperationException("NO PICTURE");
            }

            await GetUserPageById(id);

            var userpageblock = await _context.UserBlockPage.FindAsync(UserPageBlocks[target].Id);

            if (userpageblock != null)
            {
                UserPageBlock = userpageblock;

                string _txt = UploadedImage.FileName;
                var file = "./wwwroot/img/" + _txt;
                using (var fileStream = new FileStream(file, FileMode.OpenOrCreate))
                {
                    await UploadedImage.CopyToAsync(fileStream);
                    UserPageBlocks[target]._picture = UploadedImage;
                }
                UserPageBlocks[target].ImagePath = file;
            
                UserPageBlock.Overwrite(UserPageBlocks[target]);
                _context.Attach(UserPageBlock).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            else
            {
                NotFound();
            }
        }

        public async Task<IActionResult> OnPostEditBlockAsync(int id, int target)
        {
            UserPage = await _context.UserPage.FindAsync(id);
            if (UserPage == null)
            {
                return NotFound();
            }
            UserPage.Id = id;

            var userpageblock = await _context.UserBlockPage.FindAsync(UserPageBlocks[target].Id);
            
            if (userpageblock != null)
            {
                UserPageBlock = userpageblock;

                if (UploadedImage != null)
                {
                    string _txt = UploadedImage.FileName;
                    var file = "./wwwroot/img/" + _txt;
                    using (var fileStream = new FileStream(file, FileMode.OpenOrCreate))
                    {
                        await UploadedImage.CopyToAsync(fileStream);
                        UserPageBlocks[target]._picture = UploadedImage;
                    }
                    UserPageBlocks[target].ImagePath = file;
                }
                
                UserPageBlock.Overwrite(UserPageBlocks[target]);
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserPageBlock).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Redirect(GetUserPage());
        }

        public int GetNewBlockId()
        {

            return _context.UserBlockPage.Count() + 1;
        }

        private bool UserPageExists(int id)
        {
            return _context.UserPage.Any(e => e.Id == id);
        }
        private bool UserPageBlockExists(int id)
        {
            return _context.UserBlockPage.Any(e => e.Id == id);
        }

        private string GetUserPage()
        {
            string _temp = "/Admin/Edit?id=";
            if (UserPage != null)
            {
                _id = UserPage.Id;
                return _temp + _id.ToString();
            }
            else if (_id != null)
            {
                return _temp + _id.ToString();
            }

            return _temp + "0";
        }
    }
}
