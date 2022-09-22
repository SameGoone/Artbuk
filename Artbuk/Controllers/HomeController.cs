using Artbuk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Artbuk.Controllers
{
    public class HomeController : Controller
    {
        ArtbukContext _context;

        public HomeController(ArtbukContext context)
        {
            _context = context;
        }

        public IActionResult Feed()
        {
            return View(_context.Posts.OrderByDescending(p => p.CreatedDate).ToList());
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(Post? post)
        {
            if (post != null)
            {
                _context.Posts.Add(post);
                _context.SaveChanges();
            }

            return RedirectToAction("Feed");
        }

        public IActionResult DeletePost(Guid? postId)
        {
            if (postId == null)
            {
                return new NoContentResult();
            }

            var post = _context.Posts.Where(p => p.Id == postId).FirstOrDefault();
            if (post == null)
            {
                return new NoContentResult();
            }

            _context.Remove(post);
            _context.SaveChanges();
            return RedirectToAction("Feed");
        }
    }
}