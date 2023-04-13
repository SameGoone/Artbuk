using Artbuk.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class PostController : Controller
    {
        PostRepository _postRepository;
        GenreRepository _genreRepository;
        PostInGenreRepository _postInGenreRepository;
        SoftwareRepository _softwareRepository;
        PostInSoftwareRepository _postInSoftwareRepository;
        UserRepository _userRepository;
        LikeRepository _likeRepository;

        public PostController(PostRepository postRepository, PostInGenreRepository postInGenreRepository,
            GenreRepository genreRepository, PostInSoftwareRepository postInSoftwareRepository,
            SoftwareRepository softwareRepository, LikeRepository likeRepository, UserRepository userRepository)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _postInGenreRepository = postInGenreRepository;
            _genreRepository = genreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _userRepository = userRepository;
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
                id,
                Tools.GetUserId(_userRepository, User)
            );

            return View(postData);
        }
    }
}
