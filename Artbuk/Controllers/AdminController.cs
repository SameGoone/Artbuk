using Microsoft.AspNetCore.Mvc;
using Artbuk.Models;
using Artbuk.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Artbuk.Controllers
{
    [Authorize(Roles = Constants.RoleNames.Admin)]
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
