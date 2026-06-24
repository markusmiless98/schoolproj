using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PublicDatabaseAPI.Controllers;
using PublicDatabaseAPI.Data;
using PublicDatabaseAPI.Models;

namespace PublicSchoolProj.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserPageController : Controller
    {
        private readonly IUserPageController _UserPageController;

        // GET: UPageController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CustomPage(UserPage user)
        {
            return View(user);
        }


        // GET: UPageController/Create
        public ActionResult Create()
        {
            return View();
        }

    }
}
