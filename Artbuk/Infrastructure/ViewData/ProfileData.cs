namespace Artbuk.Infrastructure.ViewData
{
    public class ProfileData
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserImagePath { get; set; }
        public bool IsMe { get; set; }
        public bool IsSubscribed { get; set; }
        public List<PostFeedData> Posts { get; set; }
    }
}
