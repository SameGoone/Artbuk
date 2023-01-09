﻿using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class PostController : Controller
    {
        IPostRepository _postRepository;
        IGenreRepository _genreRepository;
        IPostInGenreRepository _postInGenreRepository;
        ISoftwareRepository _softwareRepository;
        IPostInSoftwareRepository _postInSoftwareRepository;
        IUserRepository _userRepository;
        ILikeRepository _likeRepository;

        public PostController(IPostRepository postRepository, IPostInGenreRepository postInGenreRepository, 
            IGenreRepository genreRepository, IPostInSoftwareRepository postInSoftwareRepository, 
            ISoftwareRepository softwareRepository, IUserRepository userRepository,
            ILikeRepository likeRepository = null)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _postInGenreRepository = postInGenreRepository;
            _genreRepository = genreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Post(Guid id)
        {
            var postData = new PostData
            (
                _likeRepository,
                _postRepository,
                _postInGenreRepository,
                _genreRepository,
                _postInSoftwareRepository,
                _softwareRepository,
                id
            );

            return View(postData);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreatePost()
        {
            var createPostData = new CreatePostData
            (
                _genreRepository.List(),
                _softwareRepository.List()
            );
            return View(createPostData);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreatePost(Post? post, PostInGenre? postInGenre, PostInSoftware? postInSoftware)
        {
            if (post != null && postInGenre != null && postInSoftware != null)
            {
                post.UserId = Tools.GetUserId(_userRepository, User);

                _postRepository.Add(post);
                postInGenre.PostId = post.Id;
                postInSoftware.PostId = post.Id;
                _postInGenreRepository.Add(postInGenre);
                _postInSoftwareRepository.Add(postInSoftware);
            }

            return RedirectToAction("Feed", "Feed");
        }
    }
}