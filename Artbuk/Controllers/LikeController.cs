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

        /// <summary>
        /// Проставить лайк
        /// </summary>
        /// <param name="postId">Идентификатор поста.</param>
        /// <param name="likeCheckboxState">Значение чекбокса лайка после нажатия пользователем.</param>
        /// <returns>json с данными о лайках.</returns>
        [Authorize]
        [HttpPost]
        public IActionResult AddLike(Guid? postId, bool? likeCheckboxState)
        {
            if (postId == null)
            {
                return BadRequest($"Идентификатор поста пустой!");
            }

            if (likeCheckboxState == null)
            {
                return BadRequest($"Неизвестное значение чекбокса!");
            }

            var userId = Tools.GetUserId(_userRepository, User);
            var like = _likeRepository.GetLikeOnPostByUser(postId.Value, userId);

            // Значение лайка после работы метода.
            var isLikedResult = "false";

            if (like == null)
            {
                // Если попытка поставить лайк, и лайка еще нет, добавляем лайк.
                if (likeCheckboxState.Value)
                {
                    _likeRepository.Create(postId.Value, userId);
                    isLikedResult = "true";
                }

                // Если попытка поставить лайк, и лайк уже стоит, обновляем чекбокс лайка, не добавляя лайк.
                else
                {
                    isLikedResult = "false";
                }
            }
            else
            {
                // Если попытка поставить лайк, но лайк уже стоит, обновляем чекбокс лайка, не добавляя лайк.
                if (likeCheckboxState.Value)
                {
                    isLikedResult = "true";
                }

                // Если попытка снять лайк, и лайк стоит, снимаем лайк.
                else
                {
                    _likeRepository.Remove(like);
                    isLikedResult = "false";
                }
            }

            var likesCount = _likeRepository.GetPostLikesCount(postId.Value).ToString();
            var json = $"{{\"likesCount\": {likesCount}, \"isLiked\": {isLikedResult}}}";
            return Content(json);
        }
    }
}
