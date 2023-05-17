using Artbuk.Infrastructure;
using Artbuk.Infrastructure.ViewData;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    [Authorize(Roles = Constants.RoleNames.Admin)]
    public class UsersAdminController : Controller
    {
        UserRepository _userRepository;
        RoleRepository _roleRepository;
        CommentRepository _commentRepository;
        ChatMessageRepository _chatMessageRepository;
        PostRepository _postRepository;
        PostInGenreRepository _postInGenreRepository;
        PostInSoftwareRepository _postInSoftwareRepository;
        ImageInPostRepository _imageInPostRepository;
        SubscriptionRepository _subscriptionRepository;

        public UsersAdminController(
            UserRepository userRepository,
            RoleRepository roleRepository,
            CommentRepository commentRepository,
            ChatMessageRepository chatMessageRepository,
            PostRepository postRepository,
            PostInGenreRepository postInGenreRepository,
            PostInSoftwareRepository postInSoftwareRepository,
            ImageInPostRepository imageInPostRepository,
            SubscriptionRepository subscriptionRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _commentRepository = commentRepository;
            _chatMessageRepository = chatMessageRepository;
            _postRepository = postRepository;
            _postInGenreRepository = postInGenreRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _imageInPostRepository = imageInPostRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = _userRepository.GetAll();

            var userData = new List<UserAdminPanelData>();

            var currentUserId = Tools.GetUserId(_userRepository, User);

            foreach (var user in users)
            {
                userData.Add(new UserAdminPanelData
                {
                    User = user,
                    IsAdmin = user.RoleId == Tools.GetRoleId(_roleRepository, "Admin"),
                    IsMe = user.Id == currentUserId
                });
            }

            return View(userData);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new List<string> { string.Empty, string.Empty });
        }

        [HttpGet]
        public IActionResult GetUser(Guid? userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            var user = _userRepository.GetById(userId.Value);

            var userData = new UserAdminPanelData { User = user, IsAdmin = user.RoleId == Tools.GetRoleId(_roleRepository, "Admin") };

            return View("User", userData);
        }

        [HttpPost]
        public IActionResult Update(User? user, string? isAdmin)
        {
            if (user == null)
            {
                return BadRequest();
            }

            user.Password = user.Password != null
                ? Tools.HashPassword(user.Password)
                : _userRepository.GetHashedPasswordById(user.Id);

            user.RoleId = isAdmin == "Admin"
                ? Tools.GetRoleId(_roleRepository, "Admin")
                : Tools.GetRoleId(_roleRepository, "User");

            _userRepository.Update(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateUser(User? user, string? isAdmin)
        {
            if (user != null)
            {
                var userData = new List<string> { user.Name, user.Email };

                if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Password))
                {
                    ViewBag.Message = "Заполните поля!";
                    return View(userData);
                }
                else if (string.IsNullOrEmpty(user.Name))
                {
                    ViewBag.Message = "Введите логин!";
                    return View(userData);
                }
                else if (string.IsNullOrEmpty(user.Password))
                {
                    ViewBag.Message = "Введите пароль!";
                    return View(userData);
                }

                var checkUserName = _userRepository.CheckUserExistsWithName(user.Name);
                var checkUserEmail = _userRepository.CheckUserExistsWithEmail(user.Email);

                if (checkUserName)
                {
                    ViewBag.Message = "Пользователь с таким логином уже существует.";
                    return View(userData);
                }
                else if (checkUserEmail)
                {
                    ViewBag.Message = "Пользователь с такой почтой уже существует.";
                    return View(userData);
                }
                else
                {
                    var roleId = !string.IsNullOrEmpty(isAdmin)
                        ? _roleRepository.GetAdminRoleId()
                        : _roleRepository.GetUserRoleId();

                    user.RoleId = roleId.Value;
                    user.Password = Tools.HashPassword(user.Password);

                    _userRepository.Add(user);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Guid? userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            _chatMessageRepository.RemoveMessagesByUserId(userId.Value);
            _commentRepository.RemoveCommentsByUserId(userId.Value);

            var userPosts = _postRepository.GetPostsByUserId(userId.Value);

            var postsIds = userPosts.Select(p => p.Id).ToList();

            _imageInPostRepository.RemoveByPosts(userPosts);
            _postInGenreRepository.RemoveByPosts(userPosts);
            _postInSoftwareRepository.RemoveByPosts(userPosts);
            _subscriptionRepository.RemoveSubcribedToIdByUserId(userId.Value);
            _subscriptionRepository.RemoveSubcribedByIdByUserId(userId.Value);
            _postRepository.RemovePostsByUserId(userId.Value);

            var user = _userRepository.GetById(userId.Value);

            if (user == null)
            {
                return BadRequest();
            }

            _userRepository.Delete(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult RemoveAllUserComments(Guid? userId)
        {
            if (userId == Guid.Empty || userId == null)
            {
                return BadRequest();
            }

            _commentRepository.RemoveCommentsByUserId(userId.Value);

            return RedirectToAction("Index");
        }
    }
}
