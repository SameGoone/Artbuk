using Artbuk.Core.Interfaces;
using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Feed(Guid genreId)
        {
            var postsIds = _postInGenreRepository.GetPostIdsByGenreId(genreId);
            var posts = _postRepository.GetByIds(postsIds);

            var feedData = new FeedData
            (
                _postRepository, 
                _genreRepository.List(), 
                posts, 
                _softwareRepository.List()
            );

            return View(feedData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreatePost()
        {
            var createPostData = new CreatePostData
            (
                _genreRepository.List(),
                _softwareRepository.List()
            );
            return View(createPostData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreatePost(Post? post, PostInGenre? postInGenre, PostInSoftware? postInSoftware)
        {
            if (post != null && postInGenre != null && postInSoftware != null)
            {
                post.UserId = Tools.GetUserId(_userRepository, User);

                _postRepository.Add(post);
                postInGenre.PostId = post.Id;
                postInSoftware.PostId = post.Id;
                _postInGenreRepository.Add(postInGenre);
                _postInSoftwareRepository.Add(postInSoftware);
            }

            return RedirectToAction("Feed");
        }
    }
}
