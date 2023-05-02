using Artbuk.Infrastructure;
using Artbuk.Infrastructure.ViewData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Artbuk.Controllers
{
    public class UsersController : Controller
    {
        UserRepository _userRepository;
        SubscriptionRepository _subscriptionRepository;
        PostRepository _postRepository;
        ImageInPostRepository _imageInPostRepository;

        const string _usersPageHeader = "Пользователи системы";
        const string _subscribtionsPageHeader = "Мои подписки";
        const string _subscribersPageHeader = "Мои подпичешники";

        public UsersController(UserRepository userRepository, 
            SubscriptionRepository subscriptionRepository, 
            PostRepository postRepository, 
            ImageInPostRepository imageInPostRepository)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _postRepository = postRepository;
            _imageInPostRepository = imageInPostRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Users()
        {
            var currentUserId = Tools.GetUserId(_userRepository, User);

            var usersData = new UsersData()
            {
                PageHeader = _usersPageHeader,
                Users = _userRepository
                    .GetAll()
                    .Select(u => new ProfileData(u, currentUserId, _subscriptionRepository, _postRepository, _imageInPostRepository))
                    .ToList(),
            };

            return View(usersData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Subscribtions(Guid userId)
        {
            var currentUserId = Tools.GetUserId(_userRepository, User);
            var subcribedToIds = _subscriptionRepository.GetSubcribedToIds(userId);

            var subscribtionsData = new UsersData()
            {
                PageHeader = _subscribtionsPageHeader,
                Users = _userRepository
                    .GetByIds(subcribedToIds)
                    .Select(u => new ProfileData(u, currentUserId, _subscriptionRepository, _postRepository, _imageInPostRepository))
                    .ToList(),
            };

            return View("Users", subscribtionsData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Subscribers(Guid userId)
        {
            var currentUserId = Tools.GetUserId(_userRepository, User);
            var subcribedByIds = _subscriptionRepository.GetSubcribedByIds(userId);

            var subscribersData = new UsersData()
            {
                PageHeader = _subscribersPageHeader,
                Users = _userRepository
                    .GetByIds(subcribedByIds)
                    .Select(u => new ProfileData(u, currentUserId, _subscriptionRepository, _postRepository, _imageInPostRepository))
                    .ToList(),
            };

            return View("Users", subscribersData);
        }
    }
}
