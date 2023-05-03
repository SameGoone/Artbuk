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
        public IActionResult CreatePost(Post? post, PostInGenre? postInGenre, PostInSoftware? postInSoftware, IFormFile? formFile)
        {
            if (post != null && post.Body != null && postInGenre != null && postInSoftware != null && formFile != null)
            {
                post.UserId = Tools.GetUserId(_userRepository, User);

                _postRepository.Add(post);
                postInGenre.PostId = post.Id;
                postInSoftware.PostId = post.Id;
                var filePath = Tools.SavePostImage(formFile, post.UserId, post.Id);
                _postInGenreRepository.Add(postInGenre);
                _postInSoftwareRepository.Add(postInSoftware);
                _imageInPostRepository.Add(new ImageInPost { ImagePath = filePath, PostId = post.Id});
            }
            
            return RedirectToAction("Feed");
        }
    }
}
