using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PublicSchoolProj.Classes;
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

        public List<SelectListItem> _linkOptions { get; set; }

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
            
            _linkOptions = await GetListOfItems();

            if (id != null)
            {
                _id = (int)id;
            }

            return Page();
        }

        public async Task<List<SelectListItem>> GetListOfItems()
        {
            if (_linkOptions != null) return _linkOptions;

            return await _context.UserPage.Select(a =>
            new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.title
            }).ToListAsync();
        }

        public string GetTitleFromLink(string _str)
        {
            if (_linkOptions != null)
            {
                foreach (var item in _linkOptions)
                {
                    if (item.Value == _str)
                    {
                        return item.Text;
                    }
                }
            }


            return null;
        }

        private async Task GetUserPageById(int id, bool get_blocks = true)
        {
            var userpage = await _context.UserPage.FirstOrDefaultAsync(m => m.Id == id);
            if (userpage == null)
            {
                throw new Exception("Failed to load the user page with ID " + id);
            }
            UserPage = userpage;

            if (get_blocks == true)
            {
                await GetUserBlockPageById(id);
            }
        }

        private async Task GetUserBlockPageById(int id)
        {
            UserPageBlocks = await _context.UserBlockPage.Where(e => e.UserPageId == id).ToListAsync();
            if (UserPageBlocks == null)
            {
                throw new Exception("Failed to load the user blocks from page with ID " + id);
            }
            else if (UserPage != null)
            {
                foreach (var item in UserPageBlocks)
                {
                    if (!UserPage.GetBlocks().Contains(item))
                    {
                        UserPage.GetBlocks().Add(item);
                    }
                }
            }
        }

        public async Task<PreviewModel> MakePreview()
        {
            PreviewModel _prev = new PreviewModel(_context);
            await _prev.OnGetAsync(UserPage.Id);
            return _prev;
        }

        public async Task<UserPage> PreviewUserPage()
        {
            var _prev = await _context.UserPage.Where(m => m.Id == UserPage.Id).FirstAsync();

            if (_prev == null) return null;

            UserPage _page = new UserPage();
            _page = _prev;

            List<Links> _linkedList = await SetUpLinks();
            if (_linkedList != null)
            {
                _page._listOfLinks = _linkedList;
            }

            return _prev;
        }

        private async Task<List<Links>> SetUpLinks()
        {
            if (UserPage == null) return null;
            if (UserPage.links == null) return null;

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
                return _links;
            }

            return null;
        }
        private string GetLink(string _id)
        {
            if (_id == null) return "";
            return "Admin/Edit?id=" + _id;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            await GetUserBlockPageById(UserPage.Id);

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
            await GetUserPageById(id, false);

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

            var userblock = UserPage.GetBlock(target);

            if (userblock == null)
            {
                NotFound();
            }
            var userpageblock = await _context.UserBlockPage.Where(m => m.Id == userblock.Id).FirstOrDefaultAsync();

            if (userpageblock != null)
            {
                UserPageBlock = userpageblock;

                string _txt = UploadedImage.FileName;
                var file = "./wwwroot/img/" + _txt;
                using (var fileStream = new FileStream(file, FileMode.OpenOrCreate))
                {
                    await UploadedImage.CopyToAsync(fileStream);
                    UserPageBlock._picture = UploadedImage;
                }
                UserPageBlock.ImagePath = file;
                
                userpageblock.Overwrite(UserPageBlock);
                _context.Attach(userpageblock).State = EntityState.Modified;


                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPageBlockExists(userpageblock.Id))
                    {
                        NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                NotFound();
                throw new Exception("W");
            }
        }

        public async Task<IActionResult> OnPostEditBlockAsync(int id, int target)
        {
            await GetUserPageById(id, false);

            var userpageblock = await _context.UserBlockPage.FindAsync(target);

            userpageblock.Overwrite(UserPageBlock);

            _context.Attach(userpageblock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPageBlockExists(UserPageBlock.Id))
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
        public async Task<IActionResult> OnPostLinkAsync(int id)
        {
            await GetUserPageById(id, false);

            UserPage.AddLinks();

            _context.Attach(UserPage.links).State = EntityState.Modified;

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

        public async Task<ActionResult> OnPostLinkDeleteAsync(int id, int target)
        {
            await GetUserPageById(id, false);

            int i = UserPage.GetLinks().Count;

            if (!UserPage.RemoveLatestLinkById(target))
            {
                return NotFound();
            }

            if (i == UserPage.GetLinks().Count)
            {
                throw new ArgumentException("Delete failed to occur, contact administrator");
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

        public async Task<int> GetNewBlockId()
        {
            var _listOfBlocks = await _context.UserBlockPage.ToListAsync();

            if (_listOfBlocks != null)
            {
                int i = 0;
                foreach (var item in _listOfBlocks)
                {
                    if (item.Id == i)
                    {
                        i = item.Id + 1;
                    }
                }
                return i;
            }
            else
            {
                return _context.UserBlockPage.Count() + 1;
            }
        }

        private bool UserPageExists(int id)
        {
            return _context.UserPage.Any(e => e.Id == id);
        }
        private bool UserPageBlockExists(int id)
        {
            return _context.UserBlockPage.Any(e => e.Id == id);
        }

        private string TranslateLinkToString(SelectListItem item)
        {
            return item.Value;
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
