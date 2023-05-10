using Artbuk.Infrastructure;
using Artbuk.Infrastructure.ViewData;
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
        ImageInPostRepository _imageInPostRepository;
        RoleRepository _roleRepository;

        public PostController(
            PostRepository postRepository, 
            PostInGenreRepository postInGenreRepository,
            GenreRepository genreRepository,
            PostInSoftwareRepository postInSoftwareRepository,
            SoftwareRepository softwareRepository, 
            LikeRepository likeRepository, 
            UserRepository userRepository,
            CommentRepository commentRepository, 
            ImageInPostRepository imageInPostRepository,
            RoleRepository roleRepository)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _postInGenreRepository = postInGenreRepository;
            _genreRepository = genreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _imageInPostRepository = imageInPostRepository;
            _roleRepository = roleRepository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Post(Guid? postId)
        {
            if (postId == null)
            {
                return BadRequest($"Идентификатор поста пустой!");
            }

            var postData = new PostPageData
            (
                postId.Value,
                Tools.GetUserId(_userRepository, User),
                _likeRepository,
                _postRepository,
                _postInGenreRepository,
                _genreRepository,
                _postInSoftwareRepository,
                _softwareRepository,
                _commentRepository,
                _imageInPostRepository,
                _roleRepository,
                _userRepository
            );

            return View(postData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddComment(Guid? postId, string? body)
        {
            if (postId == null)
            {
                return BadRequest($"Идентификатор поста пустой!");
            }

            if (string.IsNullOrEmpty(body))
            {
                return NoContent();
            }

            var currentUserId = Tools.GetUserId(_userRepository, User);
            var currentUser = _userRepository.GetById(currentUserId);

            Comment comment = new Comment
            {
                PostId = postId,
                Body = body,
                UserId = currentUserId,
                CreatedOn = DateTime.Now
            };

            _commentRepository.Add(comment);
            return PartialView("PostComments", PostPageData.GetCommentsData(postId.Value, currentUser, _commentRepository, _userRepository, _roleRepository));
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteComment(Guid? commentId)
        {
            if (commentId == null)
            {
                return new NoContentResult();
            }

            var currentUserId = Tools.GetUserId(_userRepository, User);
            var currentUser = _userRepository.GetById(currentUserId);
            var comment = _commentRepository.GetById(commentId.Value);

            if (comment != null)
            {
                _commentRepository.Remove(comment);
                return PartialView("PostComments", PostPageData.GetCommentsData(comment.PostId.Value, currentUser, _commentRepository, _userRepository, _roleRepository));
            }
            else
            {
                return new NoContentResult();
            }
        }


        [Authorize]
        [HttpGet]
        public IActionResult DeletePost(Guid? postId)
        {
            if (postId == null)
            {
                return new NoContentResult();
            }

            var post = _postRepository.GetById(postId.Value);
            var imageInPost = _imageInPostRepository.GetByPostId(postId.Value);

            if (imageInPost != null)
            {
                _imageInPostRepository.Remove(imageInPost);
            }

            _commentRepository.RemoveCommentsByPostId(postId.Value);

            if (post != null)
            {
                _postRepository.Remove(post);
                return RedirectToAction("Profile", "Profile");
            }
            else
            {
                return new NoContentResult();
            }
        }


        [Authorize]
        [HttpGet]
        public IActionResult CreatePost()
        {
            var createPostData = new CreateEditPostData
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
                _imageInPostRepository.Add(new ImageInPost { ImagePath = filePath, PostId = post.Id });
            }

            return RedirectToAction("Feed");
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditPost(Guid postId)
        {
            var genreInPost = _postInGenreRepository.GetPostInGenreByPostId(postId);
            var softInPost = _postInSoftwareRepository.GetPostInSoftwareByPostId(postId);

            var postEditData = new CreateEditPostData{
                Post = _postRepository.GetById(postId),
                CurrentGenre = _genreRepository.GetById(genreInPost.GenreId),
                CurrentSoftware = _softwareRepository.GetById(softInPost.SoftwareId),
                Genres = _genreRepository.GetAll(),
                Software = _softwareRepository.GetAll()
            };

            return View(postEditData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditPost(Guid postId, Guid genreId, Guid softwareId, string body)
        {
            var genreInPost = _postInGenreRepository.GetPostInGenreByPostId(postId);
            genreInPost.GenreId = genreId;
            _postInGenreRepository.Update(genreInPost);

            var softInPost = _postInSoftwareRepository.GetPostInSoftwareByPostId(postId);
            softInPost.SoftwareId = softwareId;
            _postInSoftwareRepository.Update(softInPost);
            
            var post = _postRepository.GetById(postId);
            post.Body = body;
            _postRepository.Update(post);

            return RedirectToAction("Post", new { postId = post.Id });
        }
    }
}
