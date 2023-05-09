using Artbuk.Infrastructure;
using Artbuk.Infrastructure.ViewData;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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
        ImageInPostRepository _imageInPostRepository;

        public FeedController(PostRepository postRepository, GenreRepository genreRepository,
            PostInGenreRepository postInGenreRepository, SoftwareRepository softwareRepository,
            PostInSoftwareRepository postInSoftwareRepository, UserRepository userRepository,
            LikeRepository likeRepository, ImageInPostRepository imageInPostRepository)
        {
            _postRepository = postRepository;
            _genreRepository = genreRepository;
            _postInGenreRepository = postInGenreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
            _imageInPostRepository = imageInPostRepository;
        }

        [HttpGet]
        public IActionResult Feed(Guid? genreId)
        {
            var userId = Tools.GetUserId(_userRepository, User);

            List<Post> posts;
            if (genreId == null)
            {
                posts = _postRepository.GetAll();
            }
            else
            {
                var postsIds = _postInGenreRepository.GetPostIdsByGenreId(genreId.Value);
                posts = _postRepository.GetByIds(postsIds);
            }

            var feedData = new FeedData
            (
                _likeRepository,
                _genreRepository.GetAll(),
                posts,
                _softwareRepository.GetAll(),
                userId,
                _imageInPostRepository
            );

            return View(feedData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult SearchPosts(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return RedirectToAction("Feed");
            }

            var userId = Tools.GetUserId(_userRepository, User);

            var feedData = new FeedData
            (
                _likeRepository,
                _genreRepository.GetAll(),
                _postRepository.GetPostByContentMatch(searchText),
                _softwareRepository.GetAll(),
                userId,
                _imageInPostRepository
            );

            return View("Feed", feedData);
        }
    }
}
