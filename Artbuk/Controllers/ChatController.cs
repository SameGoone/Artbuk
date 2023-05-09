using Artbuk.Infrastructure;
using Artbuk.Infrastructure.ViewData;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class ChatController : Controller
    {
        ChatMessageRepository _chatMessageRepository;
        UserRepository _userRepository;

        public ChatController(ChatMessageRepository chatMessageRepository,
            UserRepository userRepository)
        {
            _chatMessageRepository = chatMessageRepository;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Chat(Guid? withUserId)
        {
            if (withUserId == null)
            {
                return BadRequest($"Идентификатор пользователя пустой!");
            }

            var currentUserId = Tools.GetUserId(_userRepository, User);
            var messagesData = new ChatData(currentUserId, withUserId.Value, _chatMessageRepository, _userRepository);

            return View(messagesData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddMessage(string? body, Guid? withUserId)
        {
            if (withUserId == null)
            {
                return BadRequest($"Идентификатор пользователя пустой!");
            }

            if (body == null)
            {
                return NoContent();
            }

            var currentUserId = Tools.GetUserId(_userRepository, User);

            var message = new ChatMessage
            {
                Body = body,
                ToUserId = withUserId,
                FromUserId = currentUserId,
                CreatedOn = DateTime.Now
            };

            _chatMessageRepository.Add(message);

            var messagesData = new ChatData(currentUserId, withUserId.Value, _chatMessageRepository, _userRepository);
            return PartialView("ChatMessages", messagesData);
        }
    }
}
