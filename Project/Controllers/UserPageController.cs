using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;

namespace PublicSchoolProj.Controllers
{
    public interface IUserPageController
    {
        List<UserPage> ReadAll();
        void Create(UserPage userpage);
        UserPage Read(int id);
        void Update(UserPage modifier_page);
        void UpdateBlock(UserPageBlock modifier_block, int id);
        void Delete(int id);
    }
    public class UserPageController : Controller
    {
        private readonly IMemoryCache _cache;
        public UserPageController(IMemoryCache _cache)
        {
            if (_cache == null)
            {
                
            }
        }

        /*
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult UPage(UserPage _user)
        {
            if (_user == null) return View();

            return View(_user);
        }

        public IActionResult UBlock(UserPageBlock _block)
        {
            if (_block == null) return NotFound();

            return View(_block);
        }

        public IActionResult CustomPage(UserPage user)
        {
            if (user == null) return NotFound(user);

            return View(user);
        }
        */
    }
}
