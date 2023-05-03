using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class LikeController : Controller
    {
        UserRepository _userRepository;
        LikeRepository _likeRepository;

        public LikeController(UserRepository userRepository, LikeRepository likeRepository)
        {
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddLike(Guid? postId)
        {
            if (postId == null)
            {
                return BadRequest($"Идентификатор поста пустой!");
            }

            var userId = Tools.GetUserId(_userRepository, User);
            var like = _likeRepository.GetLikeOnPostByUser(postId.Value, userId);

            if (like == null)
            {
                _likeRepository.Create(postId.Value, userId);
            }
            else
            {
                _likeRepository.Remove(like);
            }

            var likesCount = _likeRepository.GetPostLikesCount(postId.Value).ToString();
            return Content(likesCount);
        }
    }
}
