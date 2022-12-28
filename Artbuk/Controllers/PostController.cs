using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class PostController : Controller
    {
        IPostRepository _postRepository;
        IGenreRepository _genreRepository;
        IPostInGenreRepository _postInGenreRepository;
        ISoftwareRepository _softwareRepository;
        IPostInSoftwareRepository _postInSoftwareRepository;
        IUserRepository _userRepository;
        ILikeRepository _likeRepository;

        public PostController(IPostRepository postRepository, IPostInGenreRepository postInGenreRepository, 
            IGenreRepository genreRepository, IPostInSoftwareRepository postInSoftwareRepository, 
            ISoftwareRepository softwareRepository, ILikeRepository likeRepository = null)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _postInGenreRepository = postInGenreRepository;
            _genreRepository = genreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
        }

        [HttpGet]
        public IActionResult Post(Guid id)
        {
            var postData = new PostData
            (
                _likeRepository,
                _postRepository,
                _postInGenreRepository,
                _genreRepository,
                _postInSoftwareRepository,
                _softwareRepository,
                id
            );

            return View(postData);
        }
    }
}
