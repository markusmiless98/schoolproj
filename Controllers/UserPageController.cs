using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicSchoolProj.Models;

namespace PublicSchoolProj.Controllers
{
    public class UserPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UPage(UserPage _user)
        {
            if (_user == null) return View();

            return View(_user);
        }
    }
}
