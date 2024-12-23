using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [Route("action/sign-in")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
