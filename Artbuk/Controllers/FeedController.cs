using Artbuk.Infrastructure;
using Artbuk.Infrastructure.ViewData;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Artbuk.Controllers
{
    public class FeedController : Controller
    {
        PostRepository _postRepository;
        GenreRepository _genreRepository;
        PostInGenreRepository _postInGenreRepository;
        SoftwareRepository _softwareRepository;
        PostInSoftwareRepository _postInSoftwareRepository;
        UserRepository _userRepository;
        LikeRepository _likeRepository;
        ImageInPostRepository _imageInPostRepository;
        FeedTypeRepository _feedTypeRepository;
        SubscriptionRepository _subscriptionRepository;

        public FeedController(PostRepository postRepository, GenreRepository genreRepository,
            PostInGenreRepository postInGenreRepository, SoftwareRepository softwareRepository,
            PostInSoftwareRepository postInSoftwareRepository, UserRepository userRepository,
            LikeRepository likeRepository, ImageInPostRepository imageInPostRepository,
            FeedTypeRepository feedTypeRepository, SubscriptionRepository subscriptionRepository)
        {
            _postRepository = postRepository;
            _genreRepository = genreRepository;
            _postInGenreRepository = postInGenreRepository;
            _softwareRepository = softwareRepository;
            _postInSoftwareRepository = postInSoftwareRepository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
            _imageInPostRepository = imageInPostRepository;
            _feedTypeRepository = feedTypeRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpGet]
        public IActionResult Feed(Guid? genreId, Guid? feedType)
        {
            var currentUserId = Tools.GetUserId(_userRepository, User);
            feedType = feedType ?? _feedTypeRepository.GetGlobalTypeId();

            List<Guid> postsIds;

            if (genreId == null)
            {
                postsIds = _postRepository.GetAllIds();
            }
            else
            {
                postsIds = _postInGenreRepository.GetPostIdsByGenreId(genreId.Value);
            }

            List<Guid> resultPostIds = null;
            // Просмотр постов от пидписок
            if (_feedTypeRepository.IsTypeSubscriptionsOnly(feedType.Value))
            {
                resultPostIds = GetPostIdsBySubscriptionsOnly(postsIds, currentUserId);
            }
            // Просмотр понравившихся постов
            else if (_feedTypeRepository.IsTypeLiked(feedType.Value))
            {
                resultPostIds = postsIds
                    .Where(postId => _likeRepository.CheckIsPostLikedByUser(postId, currentUserId))
                    .ToList();
            }
            else if (_feedTypeRepository.IsTypeGlobal(feedType.Value))
            {
                resultPostIds = postsIds;
            }

            var posts = _postRepository.GetByIds(resultPostIds);

            var feedData = new FeedData
            (
                _genreRepository.GetAll(),
                _feedTypeRepository.GetAll(),
                posts,
                _softwareRepository.GetAll(),
                currentUserId,
                _imageInPostRepository,
                new FeedOptions
                {
                    GenreId = genreId,
                    FeedTypeId = feedType
                }
            );

            return View(feedData);
        }

        protected virtual List<Guid> GetPostIdsBySubscriptionsOnly(List<Guid> postsIds, Guid currentUserId)
        {
            var resultPostIds = new List<Guid>();

            foreach (var postId in postsIds)
            {
                var post = _postRepository.GetById(postId);
                if (_subscriptionRepository.CheckIsSubrcribedTo(currentUserId, post.UserId.Value))
                {
                    resultPostIds.Add(postId);
                }
            }

            return resultPostIds;
        }

        [Authorize]
        [HttpPost]
        public IActionResult SearchPosts(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return RedirectToAction("Feed");
            }

            var userId = Tools.GetUserId(_userRepository, User);
            var globalFeedType = _feedTypeRepository.GetGlobalTypeId();

            var feedData = new FeedData
            (
                _genreRepository.GetAll(),
                _feedTypeRepository.GetAll(),
                _postRepository.GetPostsByContentMatch(searchText),
                _softwareRepository.GetAll(),
                userId,
                _imageInPostRepository,
                new FeedOptions
                {
                    GenreId = null,
                    FeedTypeId = globalFeedType
                }
            );

            return View("Feed", feedData);
        }
    }
}
