﻿using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Controllers
{
    public class ProfileController : Controller
    {
        IPostRepository _postRepository;
        IUserRepository _userRepository;
        IRoleRepository _roleRepository;

        public ProfileController(IPostRepository postRepository, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [Authorize]
        public IActionResult Profile()
        {
            var userId = Tools.GetUserId(_userRepository, User);
            return View(_postRepository.ListByUserId(userId));
        }

        [Authorize]
        public IActionResult DeletePost(Guid? postId)
        {
            if (postId == null)
            {
                return new NoContentResult();
            }

            var post = _postRepository.GetById(postId);
            if (post == null)
            {
                return new NoContentResult();
            }

            _postRepository.Delete(post);
            return RedirectToAction("Profile");
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
                var checkUserLogin = _userRepository.CheckUserLogin(user);
                var checkUserEmail = _userRepository.CheckUserEmail(user);

                if (checkUserLogin != null)
                {
                    ViewBag.Message = "Пользователь с таким логином уже существует.";
                    return View();
                }
                else if (checkUserEmail != null)
                {
                    ViewBag.Message = "Пользователь с такой почтой уже существует.";
                    return View();
                }
                else
                {
                    var RoleId = _roleRepository.GetRoleIdAtUser();
                    user.RoleId = RoleId.Id;

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleId.Name)
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

            User? user = _userRepository.CheckUserExistence(login, password);

            if (user is null)
            {
                ViewBag.Message = "Такого пользователя не существует.";
                return View();
            };

            var RoleName = _roleRepository.GetRoleNameById(user.RoleId);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleName)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect(returnUrl ?? "/");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Feed", "Feed");
        }
    }
}
