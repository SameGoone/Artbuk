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
        public int AddLike(Guid postId)
        {
            var userId = Tools.GetUserId(_userRepository, User);
            var like = _likeRepository.GetLikeOnPostByUser(postId, userId);

            if (like == null)
            {
                _likeRepository.Create(postId, userId);
            }
            else
            {
                _likeRepository.Delete(like);
            }

            return _likeRepository.GetPostLikesCount(postId);
        }
    }
}
