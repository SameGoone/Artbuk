using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class FeedController : Controller
    {
        PostRepository _postRepository;
        GenreRepository _genreRepository;
        PostInGenreRepository _postInGenreRepository;
        SoftwareRepository _softwareRepository;
        PostInSoftwareRepository _postInSoftwareRepository;
        UserRepository _userRepository;
        LikeRepository _likeRepository;

        public FeedController(PostRepository postRepository, GenreRepository genreRepository,
            PostInGenreRepository postInGenreRepository, SoftwareRepository softwareRepository,
            PostInSoftwareRepository postInSoftwareRepository, UserRepository userRepository,
            LikeRepository likeRepository)
        {
            _postRepository = postRepository;
            _genreRepository = genreRepository;
            _postInGenreRepository = postInGenreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Feed()
        {
            var userId = Tools.GetUserId(_userRepository, User);

            if (userId == Guid.Empty)
            {
                return RedirectToAction("Logout", "Profile");
            }

            var feedData = new FeedData
            (
                _likeRepository,
                _genreRepository.GetAll(),
                _postRepository.GetAll(),
                _softwareRepository.GetAll(),
                userId
            );

            return View(feedData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Feed(Guid genreId)
        {
            var userId = Tools.GetUserId(_userRepository, User);

            var postsIds = _postInGenreRepository.GetPostIdsByGenreId(genreId);
            var posts = _postRepository.GetByIds(postsIds);

            var feedData = new FeedData
            (
                _likeRepository,
                _genreRepository.GetAll(),
                posts,
                _softwareRepository.GetAll(),
                userId
            );

            return View(feedData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreatePost()
        {
            var createPostData = new CreatePostData
            (
                _genreRepository.GetAll(),
                _softwareRepository.GetAll()
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
