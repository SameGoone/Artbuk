using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Artbuk.Controllers
{
    public struct TestForm
    {
        public string Login;
        public string Password;

        public TestForm(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }

    public class ProfileController : Controller
    {
        PostRepository _postRepository;
        UserRepository _userRepository;
        RoleRepository _roleRepository;
        ImageInPostRepository _imageInPostRepository;
        CommentRepository _commentRepository;

        public ProfileController(PostRepository postRepository, UserRepository userRepository, 
            RoleRepository roleRepository, ImageInPostRepository imageInPostRepository,
            CommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _imageInPostRepository = imageInPostRepository;
            _commentRepository = commentRepository;
        }

        [Authorize]
        public IActionResult Profile()
        {
            var userId = Tools.GetUserId(_userRepository, User);
            return View(_postRepository.GetPostsByUserId(userId));
        }

        [Authorize]
        public IActionResult DeletePost(Guid? postId)
        {
            if (postId == null)
            {
                return new NoContentResult();
            }

            var post = _postRepository.GetById(postId);
            var imageInPost = _imageInPostRepository.GetByPostId(postId);

            if (imageInPost != null)
            {
                _imageInPostRepository.Delete(imageInPost);
            }

            _commentRepository.DeleteCommentsByPostId(postId);

            if (post != null)
            {
                _postRepository.Delete(post);
                return RedirectToAction("Profile");
            }
            else
            {
                return new NoContentResult();
            }
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationAsync(User? user)
        {
            if (user != null)
            {
                if (string.IsNullOrEmpty(user.Login) && string.IsNullOrEmpty(user.Password))
                {
                    ViewBag.Message = "Заполните поля!";
                    return View();
                }
                else if (string.IsNullOrEmpty(user.Login))
                {
                    ViewBag.Message = "Введите логин!";
                    return View();
                }
                else if (string.IsNullOrEmpty(user.Password))
                {
                    ViewBag.Message = "Введите пароль!";
                    return View();
                }

                var checkUserLogin = _userRepository.CheckUserExistsWithLogin(user.Login);
                var checkUserEmail = _userRepository.CheckUserExistsWithEmail(user.Email);

                if (checkUserLogin)
                {
                    ViewBag.Message = "Пользователь с таким логином уже существует.";
                    return View();
                }
                else if (checkUserEmail)
                {
                    ViewBag.Message = "Пользователь с такой почтой уже существует.";
                    return View();
                }
                else
                {
                    var roleId = _roleRepository.GetUserRoleId();
                    user.RoleId = roleId;
                    var roleName = _roleRepository.GetRoleNameById(roleId);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)
                    };
                    // создаем объект ClaimsIdentity
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    // установка аутентификационных куки
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    _userRepository.Add(user);
                }
            }
            return RedirectToAction("Feed", "Feed");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string? returnUrl)
        {
            var form = Request.Form;

            if (string.IsNullOrEmpty(form["Login"]) && string.IsNullOrEmpty(form["Password"]))
            {
                ViewBag.Message = "Заполните поля!";
                return View();
            }
            else if (string.IsNullOrEmpty(form["Login"]))
            {
                ViewBag.Message = "Введите логин!";
                return View();
            }
            else if (string.IsNullOrEmpty(form["Password"]))
            {
                ViewBag.Message = "Введите пароль!";
                return View();
            }

            string login = form["Login"];
            string password = form["Password"];

            User? user = _userRepository.GetByCredentials(login, password);

            if (user is null)
            {
                ViewBag.Message = "Такого пользователя не существует.";
                return View();
            };

            var roleName = _roleRepository.GetRoleNameById(user.RoleId);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect(returnUrl ?? "/");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Feed", "Feed");
        }
    }
}
