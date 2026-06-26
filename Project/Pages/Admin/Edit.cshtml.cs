using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;
using PublicCssAPI.DataType;
using PublicCssAPI.Handler;
using PublicDatabaseAPI.Controllers;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;

namespace PublicSchoolProj.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserPageController _controlPage;
        private readonly UserPageBlockController _controlBlock;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
            _controlPage = context.GetUserPageController();
            _controlBlock = context.GetUserPageBlockController();
        }

        [BindProperty]
        public UserPage UserPage { get; set; } = default!;
        [BindProperty]
        public IList<UserPageBlock> UserPageBlocks { get; set; } = default!;

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        int _id { get; set; } = default!;

        public List<SelectListItem> _linkOptions { get; set; }

        public List<SelectListItem> Images { get; set; }

        [BindProperty]
        public List<string> SelectedPic { get; set; }

        public List<string> _css { get; set; }

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

            await GetUserPageById((int)id);
            if (UserPage == null)
            {
                return NotFound();
            }

            _linkOptions = await GetListOfItems();

            Images = await GetListOfImages();

            SelectedPic = new List<string>();
            foreach (var item in UserPageBlocks)
            {
                if (item.IsImagePathValid())
                {
                    SelectedPic.Add(item.ImagePath);
                }
                else
                {
                    SelectedPic.Add(null);
                }
            }


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

        private async Task<List<SelectListItem>> GetListOfImages()
        {
            if (Images != null) return Images;

            string _temp = "/wwwroot/img/";
            // TBA; User must accept access to root folder or else this bad
            string _filePath = Environment.CurrentDirectory + _temp;

            string[] allfiles = Directory.GetFiles(_filePath, "*.png", SearchOption.AllDirectories);

            if (allfiles.Count() < 1)
            {
                return null;
            }

            List<SelectListItem> _items = new List<SelectListItem>();

            int i = allfiles.Count() - 1;
            while (i > 0)
            {
                var item = allfiles[i];
                Debug.WriteLine(item.ToString());
                item = item.Split("/img/")[1];
                Debug.WriteLine(item.ToString());
                i--;
                _items.Add(
                new SelectListItem
                {
                    Text = item,
                    Value = item
                });
            }
            return _items;
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
            var userpage = await _controlPage.Read(id);

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
            UserPageBlocks = await _controlBlock.ReadAll(id);

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

                List<UserPageBlock> _sort = (List<UserPageBlock>)UserPageBlocks;
                _sort.Sort(SortByRowColumn);
            }
        }

        private int SortByRowColumn(UserPageBlock A, UserPageBlock B)
        {
            if (A.Row > B.Row)
            {
                return 1;
            }
            if (B.Row < A.Row)
            {
                return -1;
            }

            if (A.Column > B.Column)
            {
                return 1;
            }
            if (B.Column > A.Column)
            {
                return -1;
            }

            return 0;
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

            await _controlPage.Update(UserPage);

            return Redirect(GetUserPage());
        }


        [BindProperty]
        public UserPageBlock UserPageBlock { get; set; } = default!;


        public async Task<IActionResult> OnPostBlockAsync(int id)
        {
            UserPageBlock _block = new UserPageBlock();
            _block.UserPageId = UserPage.userId;

            await _controlBlock.Create(UserPageBlock);

            return Redirect(GetUserPage());
        }
        public async Task<IActionResult> OnPostDeleteBlockAsync(int id, int target)
        {
            await GetUserPageById(id, false);

            UserPage.GetBlocks().Clear();
            List<UserPageBlock> _blocks = new List<UserPageBlock>();
            _blocks = await _controlBlock.ReadAll();

            if (_blocks.Count < 1)
            {
                return NotFound();
            }

            UserPageBlock _block = UserPage.GetBlocks()[target];
            _block.UserPageId = UserPage.userId;

            var userpageblock = await _controlBlock.Read(_block.Id);
            if (userpageblock != null)
            {
                UserPageBlock = userpageblock;

                await _controlBlock.Delete(userpageblock.Id);
            }

            return Redirect(GetUserPage());
        }

        public async Task<IActionResult> OnPostAddPicAsync(int id, int target)
        {
            if (UploadedImage == null)
            {
                throw new InvalidOperationException("NO PICTURE");
            }

            await GetUserPageById(id);

            var userblock = UserPage.GetBlock(target);

            if (userblock == null)
            {
                return NotFound();
            }
            var userpageblock = await _controlBlock.Read(userblock.Id);

            if (userpageblock != null)
            {
                UserPageBlock = userpageblock;

                string _txt = UploadedImage.FileName;
                var file = "./wwwroot/img/" + _txt;
                using (var fileStream = new FileStream(file, FileMode.OpenOrCreate))
                {
                    await UploadedImage.CopyToAsync(fileStream);
                    UserPageBlock.ImagePath = UploadedImage.FileName;
                }
                
                userpageblock.OverWrite(UserPageBlock);

                await _controlBlock.Update(userpageblock);
            }
            else
            {
                return NotFound();
            }

            return Redirect(GetUserPage());
        }

        public async Task<IActionResult> OnPostUpdatePicAsync(int id, int target, string? image)
        {
            await GetUserPageById(id);

            var userpageblock = await _controlBlock.Read(target);

            if (userpageblock != null)
            {
                UserPageBlock = userpageblock;

                if (image != null)
                {
                    UserPageBlock.ImagePath = image;

                    userpageblock.OverWrite(UserPageBlock);

                    await _controlBlock.Update(userpageblock);

                    return Redirect(GetUserPage());
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostRemovePicAsync(int id, int target)
        {
            await GetUserPageById(id);

            var userblock = UserPage.GetBlock(target);

            if (userblock == null)
            {
                return NotFound();
            }
            var userpageblock = await _controlBlock.Read(userblock.Id);

            if (userpageblock != null)
            {
                UserPageBlock = userpageblock;
                UserPageBlock.ImagePath = null;

                userpageblock.OverWrite(UserPageBlock);

                await _controlBlock.Update(userpageblock);
            }
            else
            {
                return NotFound();
            }

            return Redirect(GetUserPage());
        }

        public async Task<IActionResult> OnPostEditBlockAsync(int id, int target)
        {
            await GetUserPageById(id, false);

            var userpageblock = await _controlBlock.Read(target);

            if (userpageblock == null)
            {
                return NotFound();
            }

            if (UserPageBlock.ImagePath == "NONE")
            {
                UserPageBlock.ImagePath = "";
                userpageblock.ImagePath = "";
            }

            userpageblock.OverWrite(UserPageBlock);

            await _controlBlock.Update(userpageblock);

            return Redirect(GetUserPage());
        }
        public async Task<IActionResult> OnPostEditAllBlockAsync()
        {
            if (UserPageBlocks == null)
            {
                return NotFound(UserPageBlocks);
            }
            if (UserPage == null)
            {
                return NotFound(UserPage);
            }

            foreach (var item in UserPageBlocks)
            {
                await _controlBlock.AddToList(item);
            }

            //await _controlBlock.HandleFromList(EntityState.Modified, );

            return Redirect(GetUserPage());
        }

        public async Task<IActionResult> OnPostLinkAsync(int id)
        {
            await GetUserPageById(id, false);

            UserPage.AddLinks();

            await _controlPage.Update(UserPage);

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

            await _controlPage.Update(UserPage);

            return Redirect(GetUserPage());
        }

        public async Task<int> GetNewBlockId()
        {
            var _listOfBlocks = await _controlBlock.ReadAll();

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
                var b = await _controlBlock.ReadAll();
                return (int)b.Count;
            }
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
