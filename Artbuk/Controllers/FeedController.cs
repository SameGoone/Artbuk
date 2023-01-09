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
        ILikeRepository _likeRepository;

        public FeedController(IPostRepository postRepository, IGenreRepository genreRepository, 
            IPostInGenreRepository postInGenreRepository, ISoftwareRepository softwareRepository, 
            IPostInSoftwareRepository postInSoftwareRepository, IUserRepository userRepository, 
            ILikeRepository likeRepository = null)
        {
            _postRepository = postRepository;
            _genreRepository = genreRepository;
            _postInGenreRepository = postInGenreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        [HttpGet]
        public IActionResult Feed()
        {
            var feedData = new FeedData
            (
                _likeRepository,
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
                _likeRepository, 
                _genreRepository.List(), 
                posts, 
                _softwareRepository.List()
            );

            return View(feedData);
        }
    }
}
