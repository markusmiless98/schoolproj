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
        public List<UserPage> _pages { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(List<UserPage>? _list)
        {
            if (_list != null)
            {
                _pages = _list;
            }

        }

        public async Task OnPost()
        {
            // Unsued now
        }
    }
}
