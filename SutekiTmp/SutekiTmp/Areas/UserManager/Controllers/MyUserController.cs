using Microsoft.AspNetCore.Mvc;

namespace SutekiTmp.Areas.UserManager.Controllers
{
    [Area("UserManager")]
    public class MyUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
