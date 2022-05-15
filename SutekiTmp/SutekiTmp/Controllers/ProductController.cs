using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SutekiTmp.Domain.Common.Attributes;
using SutekiTmp.Domain.Common.Authentication.Session;

namespace SutekiTmp.Controllers
{
    [Authorize(AuthenticationSchemes = SessionAuthenticationOptions.Scheme,Policy = "Premission")]
    [Menu("訂單管理")]
    public class ProductController : Controller
    {
        [Permission("檢視")]
        public IActionResult Index()
        {
            return View();
        }

        [Permission("新建")]
        public IActionResult Create()
        {
            return View();
        }

        [Permission("更新")]
        public IActionResult Update()
        {
            return View();
        }

        [Permission("刪除")]
        public IActionResult Delete()
        {
            return View();
        }
    }
}
