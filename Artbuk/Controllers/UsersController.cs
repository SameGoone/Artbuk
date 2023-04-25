using Artbuk.Infrastructure;
using Artbuk.Infrastructure.ViewData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class UsersController : Controller
    {
        UserRepository _userRepository;
        SubscriptionRepository _subscriptionRepository;

        const string _usersPageHeader = "Пользователи системы";
        const string _subscribtionsPageHeader = "Мои подписки";
        const string _subscribersPageHeader = "Мои подпичешники";

        public UsersController(UserRepository userRepository, SubscriptionRepository subscriptionRepository)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Users()
        {
            var usersData = new UsersData()
            {
                PageHeader = _usersPageHeader,
                Users = _userRepository.GetAll()
            };

            return View(usersData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Subscribtions(Guid userId)
        {
            var subcribedToIds = _subscriptionRepository.GetSubcribedToIds(userId);

            var subscribtionsData = new UsersData()
            {
                PageHeader = _subscribtionsPageHeader,
                Users = _userRepository.GetByIds(subcribedToIds)
            };

            return View("Users", subscribtionsData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Subscribers(Guid userId)
        {
            var subcribedByIds = _subscriptionRepository.GetSubcribedByIds(userId);

            var subscribersData = new UsersData()
            {
                PageHeader = _subscribersPageHeader,
                Users = _userRepository.GetByIds(subcribedByIds)
            };

            return View("Users", subscribersData);
        }
    }
}
