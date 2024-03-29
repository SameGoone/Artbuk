﻿using Artbuk.Infrastructure;
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
        LikeRepository _likeRepository;
        RoleRepository _roleRepository;

        const string _usersPageHeader = "Пользователи системы";
        const string _subscribtionsPageHeader = "Подписки пользователя {0}";
        const string _subscribersPageHeader = "Подписчики пользователя {0}:";
        const string _postLikedByHeader = "Лайкнули пост \"{0}\"";

        public UsersController(UserRepository userRepository, 
            SubscriptionRepository subscriptionRepository, 
            PostRepository postRepository, 
            ImageInPostRepository imageInPostRepository,
            LikeRepository likeRepository,
            RoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _postRepository = postRepository;
            _imageInPostRepository = imageInPostRepository;
            _likeRepository = likeRepository;
            _roleRepository = roleRepository;
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
                    .Select(u => new ProfileData(u, currentUserId, _subscriptionRepository, _postRepository, _imageInPostRepository, _roleRepository, _userRepository))
                    .ToList(),
            };

            return View(usersData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Subscribtions(Guid? userId)
        {
            if (userId == null)
            {
                return BadRequest("Пустой идентификатор пользователя!");
            }

            var currentUserId = Tools.GetUserId(_userRepository, User);
            var subcribedToIds = _subscriptionRepository.GetSubcribedToIds(userId.Value);
            var user = _userRepository.GetById(userId.Value);
            var pageHeader = string.Format(_subscribtionsPageHeader, user.Name);

            var subscribtionsData = new UsersData()
            {
                PageHeader = pageHeader,
                Users = _userRepository
                    .GetByIds(subcribedToIds)
                    .Select(u => new ProfileData(u, currentUserId, _subscriptionRepository, _postRepository, _imageInPostRepository, _roleRepository, _userRepository))
                    .ToList(),
            };

            return View("Users", subscribtionsData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Subscribers(Guid? userId)
        {
            if (userId == null)
            {
                return BadRequest("Пустой идентификатор пользователя!");
            }

            var currentUserId = Tools.GetUserId(_userRepository, User);
            var subcribedByIds = _subscriptionRepository.GetSubcribedByIds(userId.Value);
            var user = _userRepository.GetById(userId.Value);
            var pageHeader = string.Format(_subscribersPageHeader, user.Name);

            var subscribersData = new UsersData()
            {
                PageHeader = pageHeader,
                Users = _userRepository
                    .GetByIds(subcribedByIds)
                    .Select(u => new ProfileData(u, currentUserId, _subscriptionRepository, _postRepository, _imageInPostRepository, _roleRepository, _userRepository))
                    .ToList(),
            };

            return View("Users", subscribersData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult PostLikedBy(Guid? postId)
        {
            if (postId == null)
            {
                return BadRequest("Пустой идентификатор поста!");
            }

            var currentUserId = Tools.GetUserId(_userRepository, User);
            var subcribedByIds = _likeRepository.GetPostLikedByIds(postId.Value);
            var post = _postRepository.GetById(postId.Value);
            var pageHeader = string.Format(_postLikedByHeader, post.Body);

            var postLikedByData = new UsersData()
            {
                PageHeader = pageHeader,
                Users = _userRepository
                    .GetByIds(subcribedByIds)
                    .Select(u => new ProfileData(u, currentUserId, _subscriptionRepository, _postRepository, _imageInPostRepository, _roleRepository, _userRepository))
                    .ToList(),
            };

            return View("Users", postLikedByData);
        }
    }
}
