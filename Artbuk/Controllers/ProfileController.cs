using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
        SubscriptionRepository _subscriptionRepository;

        public ProfileController(PostRepository postRepository, UserRepository userRepository, 
            RoleRepository roleRepository, ImageInPostRepository imageInPostRepository,
            CommentRepository commentRepository, SubscriptionRepository subscriptionRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _imageInPostRepository = imageInPostRepository;
            _commentRepository = commentRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile(Guid userId)
        {
            var user = _userRepository.GetById(userId);
            var currentUserId = Tools.GetUserId(_userRepository, User);
            var userPosts = _postRepository.GetPostsByUserId(userId);

            var data = new ProfileData()
            {
                UserId = userId,
                UserName = user.Name,
                UserImagePath = Tools.GetImagePath(user.ImagePath),
                IsMe = currentUserId == userId,
                IsSubscribed = _subscriptionRepository.CheckIsSubrcribedTo(currentUserId, userId),

                Posts = PostFeedData.GetDataRange(userPosts, _imageInPostRepository)
            };

            return View(data);
        }

        [Authorize]
        [HttpPost]
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
                if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Password))
                {
                    ViewBag.Message = "Заполните поля!";
                    return View();
                }
                else if (string.IsNullOrEmpty(user.Name))
                {
                    ViewBag.Message = "Введите логин!";
                    return View();
                }
                else if (string.IsNullOrEmpty(user.Password))
                {
                    ViewBag.Message = "Введите пароль!";
                    return View();
                }

                var checkUserName = _userRepository.CheckUserExistsWithName(user.Name);
                var checkUserEmail = _userRepository.CheckUserExistsWithEmail(user.Email);

                if (checkUserName)
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
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
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

            if (string.IsNullOrEmpty(form["Name"]) && string.IsNullOrEmpty(form["Password"]))
            {
                ViewBag.Message = "Заполните поля!";
                return View();
            }
            else if (string.IsNullOrEmpty(form["Name"]))
            {
                ViewBag.Message = "Введите логин!";
                return View();
            }
            else if (string.IsNullOrEmpty(form["Password"]))
            {
                ViewBag.Message = "Введите пароль!";
                return View();
            }

            string name = form["Name"];
            string password = form["Password"];

            User? user = _userRepository.GetByCredentials(name, password);

            if (user is null)
            {
                ViewBag.Message = "Такого пользователя не существует.";
                return View();
            };

            var roleName = _roleRepository.GetRoleNameById(user.RoleId);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
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

        [Authorize]
        [HttpGet]
        public IActionResult ChooseUserImage(Guid? userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChooseUserImage(Guid? userId, IFormFile? formFile)
        {
            var filePath = Tools.SaveUserImage(formFile, userId);
            var user = _userRepository.GetById(userId);
            user.ImagePath = filePath;
            _userRepository.Update(user);

            return RedirectToAction("Profile", "Profile", new { userId = userId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Subscribe(Guid? followedId)
        {
            if (!followedId.HasValue)
            {
                return NoContent();
            }

            var userId = Tools.GetUserId(_userRepository, User);

            if (_subscriptionRepository.CheckIsSubrcribedTo(userId, followedId.Value))
            {
                return NoContent();
            }

            var subscribtion = new Subscription()
            {
                SubcriberId = userId,
                FollowedId = followedId,
            };

            _subscriptionRepository.Add(subscribtion);
            return RedirectToAction("Profile", new { userId = followedId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Unsubscribe(Guid? followedId)
        {
            if (!followedId.HasValue)
            {
                return NoContent();
            }

            var userId = Tools.GetUserId(_userRepository, User);
            var subscribtion = _subscriptionRepository.GetBySubscriberAndFollowed(userId, followedId.Value);

            if (subscribtion == null)
            {
                return NoContent();
            }

            _subscriptionRepository.Remove(subscribtion);
            return RedirectToAction("Profile", new { userId = followedId });
        }
    }
}
