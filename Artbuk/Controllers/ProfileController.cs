﻿using Artbuk.Infrastructure;
using Artbuk.Infrastructure.ViewData;
using Artbuk.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Security.Cryptography;

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
        public IActionResult Profile(Guid? userId)
        {
            var user = userId == Guid.Empty || userId == null
                ? _userRepository.GetById(Tools.GetUserId(_userRepository, User))
                : _userRepository.GetById(userId.Value);

            var currentUserId = Tools.GetUserId(_userRepository, User);

            var data = new ProfileData(user, currentUserId, _subscriptionRepository, _postRepository, _imageInPostRepository, _roleRepository, _userRepository)
            {
                UserId = user.Id,
                Name = user.Name,
                ImagePath = Tools.GetImagePath(user.ImagePath),
                IsMe = currentUserId == user.Id,
                IsSubscribed = _subscriptionRepository.CheckIsSubrcribedTo(currentUserId, user.Id)
            };

            return View(data);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View(new List<string> { string.Empty, string.Empty } );
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationAsync(User? user)
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
                    var roleId = _roleRepository.GetUserRoleId();
                    user.RoleId = roleId.Value;
                    user.Password = Tools.HashPassword(user.Password);
                    var roleName = _roleRepository.GetRoleNameById(roleId.Value);

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
            return View(new List<string> { string.Empty });
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string? returnUrl)
        {
            var form = Request.Form;

            var userData = new List<string> { form["Name"].ToString() };

            if (string.IsNullOrEmpty(form["Name"]) && string.IsNullOrEmpty(form["Password"]))
            {
                ViewBag.Message = "Заполните поля!";
                return View(userData);
            }
            else if (string.IsNullOrEmpty(form["Name"]))
            {
                ViewBag.Message = "Введите логин!";
                return View(userData);
            }
            else if (string.IsNullOrEmpty(form["Password"]))
            {
                ViewBag.Message = "Введите пароль!";
                return View(userData);
            }

            string name = form["Name"];
            string password = form["Password"];

            var hashedPassword = _userRepository.GetHashedPasswordByName(name);
            var resultComparison = Tools.VerifyHashedPassword(hashedPassword, password);

            if (resultComparison == false)
            {
                ViewBag.Message = "Такого пользователя не существует.";
                return View(userData);
            };

            User? user = _userRepository.GetByName(name);

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
            if (userId == null)
            {
                return BadRequest("Пустой идентификатор пользователя!");
            }

            ViewBag.UserId = userId;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChooseUserImage(Guid? userId, IFormFile? formFile)
        {
            if (userId == null)
            {
                return BadRequest("Пустой идентификатор пользователя!");
            }

            if (formFile == null)
            {
                return BadRequest("Передано некорректное изображение!");
            }

            var filePath = Tools.SaveUserImage(formFile, userId);
            var user = _userRepository.GetById(userId.Value);
            user.ImagePath = filePath;
            _userRepository.Update(user);

            return RedirectToAction("Profile", "Profile", new { userId = userId });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Subscribe(Guid? subcribeToId)
        {
            if (subcribeToId == null)
            {
                return NotFound("Пустой идентификатор");
            }

            var userId = Tools.GetUserId(_userRepository, User);

            if (_subscriptionRepository.CheckIsSubrcribedTo(userId, subcribeToId.Value))
            {
                return NotFound($"Пользователь {userId} уже подписан на пользователя {subcribeToId}!");
            }

            var subscribtion = new Subscription()
            {
                SubcribedById = userId,
                SubcribedToId = subcribeToId,
            };

            _subscriptionRepository.Add(subscribtion);
            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Unsubscribe(Guid? unsubcribeToId)
        {
            if (unsubcribeToId == null)
            {
                return NotFound("Пустой идентификатор");
            }

            var userId = Tools.GetUserId(_userRepository, User);
            var subscribtion = _subscriptionRepository.GetBySubscribePair(userId, unsubcribeToId.Value);

            if (subscribtion == null)
            {
                return NotFound($"Пользователь {userId} уже отписан от пользователя {unsubcribeToId}!");
            }

            _subscriptionRepository.Remove(subscribtion);
            return NoContent();
        }
    }
}
