using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Artbuk.Controllers
{
    public class FeedController : Controller
    {
        IPostRepository _postRepository;
        IGenreRepository _genreRepository;
        IPostInGenreRepository _postInGenreRepository;

        public FeedController(IPostRepository postRepository, IGenreRepository genreRepository, IPostInGenreRepository postInGenreRepository)
        {
            _postRepository = postRepository;
            _genreRepository = genreRepository;
            _postInGenreRepository = postInGenreRepository;
        }

        [HttpGet]
        public IActionResult Feed()
        {
            var feedData = new FeedData
            (
                _postRepository, 
                _genreRepository.List(), 
                _postRepository.ListAll()
            );

            return View(feedData);
        }

        [HttpPost]
        public IActionResult Feed(Guid genreId)
        {
            var postsIds = _postInGenreRepository.GetPostIdsByGenreId(genreId);
            var posts = _postRepository.GetByIds(postsIds);

            var feedData = new FeedData(_postRepository, _genreRepository.List(), posts);

            return View(feedData);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View(_genreRepository.List());
        }

        [HttpPost]
        public IActionResult CreatePost(Post? post, PostInGenre? postInGenre)
        {
            if (post != null)
            {
                _postRepository.Add(post);

                if (postInGenre != null)
                {
                    postInGenre.PostId = post.Id;
                    _postInGenreRepository.Add(postInGenre);
                }
            }

            return RedirectToAction("Feed");
        }
    }
}
