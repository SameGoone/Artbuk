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
        public bool IsAdmin { get; set; }
        public List<PostFeedData> Posts { get; set; }
        public int SubscriprionsCount { get; set; }
        public int SubscribersCount { get; set; }

        public ProfileData(User user, Guid currentUserId,
            SubscriptionRepository subscriptionRepository,
            PostRepository postRepository,
            ImageInPostRepository imageInPostRepository,
            RoleRepository roleRepository, 
            UserRepository userRepository)
        {
            var userPosts = postRepository.GetPostsByUserId(user.Id);
            var currentUser = userRepository.GetById(currentUserId);

            UserId = user.Id;
            Name = user.Name;
            ImagePath = Tools.GetImagePath(user.ImagePath);
            IsMe = currentUserId == user.Id;
            IsSubscribed = subscriptionRepository.CheckIsSubrcribedTo(currentUserId, user.Id);
            IsAdmin = currentUser.RoleId == Tools.GetRoleId(roleRepository, "Admin");
            Posts = PostFeedData.GetDataRange(userPosts, imageInPostRepository);
            SubscriprionsCount = subscriptionRepository.GetSubcribedToCount(user.Id);
            SubscribersCount = subscriptionRepository.GetSubcribedByCount(user.Id);
        }
    }
}
