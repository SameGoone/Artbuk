using Artbuk.Infrastructure;
using Artbuk.Infrastructure.ViewData;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Artbuk.Controllers
{
    [Authorize(Roles = Constants.RoleNames.Admin)]
    public class UsersAdminController : Controller
    {
        UserRepository _userRepository;
        RoleRepository _roleRepository;
        CommentRepository _commentRepository;

        public UsersAdminController(UserRepository userRepository, RoleRepository roleRepository, CommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _commentRepository = commentRepository;
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

            _commentRepository.RemoveCommentsByUserId(userId.Value);
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
