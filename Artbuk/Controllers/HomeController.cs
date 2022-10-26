using Artbuk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace Artbuk.Controllers
{
    public class HomeController : Controller
    {
        ArtbukContext _context;

        public HomeController(ArtbukContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Feed()
        {
            var feedData = new FeedData() 
            { 
                Posts = _context.Posts.OrderByDescending(p => p.CreatedDate).ToList(),
                Genres = _context.Genres
            };

            return View(feedData);
        }

        [HttpPost]
        public IActionResult Feed(Guid genreId)
        {
            var postsIds = _context.PostInGenres
                .Where(p => p.GenreId == genreId)
                .Select(p => p.PostId)
                .Distinct()
                .ToList();

            var posts = _context.Posts.Where(p => postsIds.Contains(p.Id));

            var feedData = new FeedData()
            {
                Posts = posts,
                Genres = _context.Genres
            };

            return View(feedData);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            var feedData = new FeedData()
            {
                Genres = _context.Genres
            };
            return View(feedData);
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