using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
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
        CommentRepository _commentRepository;

        public PostController(PostRepository postRepository, PostInGenreRepository postInGenreRepository,
            GenreRepository genreRepository, PostInSoftwareRepository postInSoftwareRepository,
            SoftwareRepository softwareRepository, LikeRepository likeRepository, UserRepository userRepository,
            CommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _postInGenreRepository = postInGenreRepository;
            _genreRepository = genreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Post(Guid postId)
        {
            var postData = new PostPageData
            (
                postId,
                Tools.GetUserId(_userRepository, User),
                _likeRepository,
                _postRepository,
                _postInGenreRepository,
                _genreRepository,
                _postInSoftwareRepository,
                _softwareRepository,
                _commentRepository
            );

            return View(postData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddComment(Guid postId, string body)
        {
            Comment comment = new Comment
            {
                PostId = postId,
                Body = body,
                UserId = Tools.GetUserId(_userRepository, User),
                CreatedOn = DateTime.Now
            };

            _commentRepository.Add(comment);
            return RedirectToAction("Post", new { postId = postId });
        }
    }
}
