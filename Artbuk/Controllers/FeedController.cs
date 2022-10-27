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
                _genreRepository.ListAsync().Result, 
                _postRepository.ListAsync().Result
            );

            return View(feedData);
        }

        [HttpPost]
        public async Task<IActionResult> FeedAsync(Guid genreId)
        {
            var postsIds = await _postInGenreRepository.GetPostIdsByGenreIdAsync(genreId);
            var posts = await _postRepository.GetByIdsAsync(postsIds);

            var feedData = new FeedData(_postRepository, _genreRepository.ListAsync().Result, posts);

            return View(feedData);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View(_genreRepository.ListAsync().Result);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostAsync(Post? post, PostInGenre? postInGenre)
        {
            if (post != null)
            {
                await _postRepository.AddAsync(post);

                if (postInGenre != null)
                {
                    postInGenre.PostId = post.Id;
                    await _postInGenreRepository.AddAsync(postInGenre);
                }
            }

            return RedirectToAction("Feed");
        }
    }
}
