using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class FeedController : Controller
    {
        IPostRepository _postRepository;
        IGenreRepository _genreRepository;
        IPostInGenreRepository _postInGenreRepository;
        ISoftwareRepository _softwareRepository;
        IPostInSoftwareRepository _postInSoftwareRepository;
        IUserRepository _userRepository;

        public FeedController(IPostRepository postRepository, IGenreRepository genreRepository, IPostInGenreRepository postInGenreRepository, ISoftwareRepository softwareRepository, IPostInSoftwareRepository postInSoftwareRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _genreRepository = genreRepository;
            _postInGenreRepository = postInGenreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Feed()
        {
            var feedData = new FeedData
            (
                _postRepository,
                _genreRepository.List(),
                _postRepository.ListAll(),
                _softwareRepository.List()
            );

            return View(feedData);
        }

        [HttpPost]
        public IActionResult Feed(Guid genreId, Guid softwareId)
        {
            var postsIds = _postInGenreRepository.GetPostIdsByGenreId(genreId);
            var posts = _postRepository.GetByIds(postsIds);

            var feedData = new FeedData(_postRepository, _genreRepository.List(), posts, _softwareRepository.List());

            return View(feedData);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            var feedData = new FeedData
            (
                _postRepository,
                _genreRepository.List(),
                _postRepository.ListAll(),
                _softwareRepository.List()
            );
            return View(feedData);
        }

        [HttpPost]
        public IActionResult CreatePost(Post? post, PostInGenre? postInGenre, PostInSoftware? postInSoftware)
        {
            if (post != null)
            {
                _postRepository.Add(post);

                if (postInGenre != null && postInSoftware != null)
                {
                    postInGenre.PostId = post.Id;
                    postInSoftware.PostId = post.Id;
                    _postInGenreRepository.Add(postInGenre);
                    _postInSoftwareRepository.Add(postInSoftware);
                }
            }

            return RedirectToAction("Feed");
        }
    }
}
