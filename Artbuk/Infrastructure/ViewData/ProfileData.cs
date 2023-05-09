using Artbuk.Models;

namespace Artbuk.Infrastructure.ViewData
{
    public class ProfileData
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public bool IsMe { get; set; }
        public bool IsSubscribed { get; set; }
        public List<PostFeedData> Posts { get; set; }
        public int SubscriprionsCount { get; set; }
        public int SubscribersCount { get; set; }

        public ProfileData(User user, Guid currentUserId,
            SubscriptionRepository subscriptionRepository,
            PostRepository postRepository,
            ImageInPostRepository imageInPostRepository)
        {
            var userPosts = postRepository.GetPostsByUserId(user.Id);

            UserId = user.Id;
            Name = user.Name;
            ImagePath = Tools.GetImagePath(user.ImagePath);
            IsMe = currentUserId == user.Id;
            IsSubscribed = subscriptionRepository.CheckIsSubrcribedTo(currentUserId, user.Id);
            Posts = PostFeedData.GetDataRange(userPosts, imageInPostRepository);
            SubscriprionsCount = subscriptionRepository.GetSubcribedToCount(user.Id);
            SubscribersCount = subscriptionRepository.GetSubcribedByCount(user.Id);
        }
    }
}
