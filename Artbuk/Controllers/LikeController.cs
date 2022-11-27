using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class LikeController : Controller
    {
        IUserRepository _userRepository;
        ILikeRepository _likeRepository;

        public LikeController(IUserRepository userRepository, ILikeRepository likeRepository)
        {
            _userRepository = userRepository;
            _likeRepository = likeRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddLike()
        {
            return RedirectToAction("Feed", "Feed");
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddLike(Guid postId, bool isLiked)
        {
            var userId = Tools.GetUserId(_userRepository, User);
            var like = _likeRepository.GetLikeOnPostByUser(postId, userId);

            if (isLiked)
            {
                if (like == null)
                {
                    Like newLike = new Like(postId, userId);
                    _likeRepository.Add(newLike);
                }
            }
            else
            {
                if (like != null)
                {
                    _likeRepository.Delete(like);
                }
            }

            return RedirectToAction("Feed", "Feed");
        }
    }
}
