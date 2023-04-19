using Artbuk.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class UsersController : Controller
    {
        UserRepository _userRepository;
        SubscriptionRepository _subscriptionRepository;

        public UsersController(UserRepository userRepository, SubscriptionRepository subscriptionRepository)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Users()
        {
            return View(_userRepository.GetAll());
        }

        [Authorize]
        [HttpGet]
        public IActionResult Subscribtions(Guid userId)
        {
            var followedIds = _subscriptionRepository.GetFollowedIds(userId);
            return View(_userRepository.GetByIds(followedIds));
        }
    }
}
