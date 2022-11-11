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
            Like likeCkeck = _likeRepository.CheckIsLiked(postId, userId);

            if (isLiked == true)
            {
                if (likeCkeck == null)
                {
                    Like like = new Like(postId, userId);
                    _likeRepository.Add(like);
                }
            }
            else
            {
                if (likeCkeck != null)
                {
                    _likeRepository.Delete(likeCkeck);
                }
            }

            return RedirectToAction("Feed", "Feed");
        }
    }
}
