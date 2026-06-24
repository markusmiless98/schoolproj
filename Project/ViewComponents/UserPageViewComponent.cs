using Microsoft.AspNetCore.Mvc;
using PublicDatabaseAPI.Models;

namespace PublicSchoolProj.ViewComponents
{
    public class UserPageViewComponent : ViewComponent
    {
        private UserPage _userPage;

        public UserPageViewComponent(UserPage userService)
        {
            _userPage = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var users = await _userPage.GetPagesAsync();
            if (_userPage == null) return null; // For now
            var users = _userPage;
            return View(users);
        }
    }
}
