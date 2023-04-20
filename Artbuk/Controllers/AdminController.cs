using Microsoft.AspNetCore.Mvc;
using Artbuk.Models;
using Artbuk.Infrastructure;

namespace Artbuk.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
