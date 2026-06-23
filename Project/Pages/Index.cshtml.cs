using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;

namespace PublicSchoolProj.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string PostDetails { get; set; }

        [BindProperty]
        public IFormFile UploadedImage { get; set; }
        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public UserPage _UserPage { get; set; }

        [BindProperty]
        public UserPageManager PageManager { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            if (PageManager == null)
            {
                PageManager = new UserPageManager();
            }
        }

        public void OnGet()
        {
            if (PageManager == null)
            {
                PageManager = new UserPageManager();
            }
        }

        public async Task OnPost()
        {
            if (UploadedImage == null && Description == null) return;


            UserPage _page = PageManager._UserPage;
            PageManager.CreateNewBlock(true);

            if (UploadedImage != null)
            {
                string _txt = UploadedImage.FileName;
                var file = "./wwwroot/img/" + _txt;
                using (var fileStream = new FileStream(file, FileMode.OpenOrCreate))
                {
                    await UploadedImage.CopyToAsync(fileStream);
                    _txt = file.ToString();
                }
            }
            if (Description != null)
            {
                PageManager.SetDescriptionOfBlock(Description);
            }

            if (PageManager.ValidateOrDeleteBlock())
            {
                _page = PageManager._UserPage;
                if (_page.IsValidPage())
                {
                    _UserPage = _page;
                }
            }
        }
    }
}
