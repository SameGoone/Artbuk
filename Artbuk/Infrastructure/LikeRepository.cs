using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class LikeRepository
    {
        private readonly ArtbukContext _dbContext;
        public LikeRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Like? GetById(Guid likeId)
        {
            return _dbContext.Likes
                .FirstOrDefault(i => i.Id == likeId);
        }

        public List<Like> GetLikesByUserId(Guid userId)
        {
            return _dbContext.Likes
                .Where(i => i.UserId == userId)
                .ToList();
        }

        public Guid Create(Guid postId, Guid userId)
        {
            Like like = new Like(postId, userId);
            _dbContext.Likes.Add(like);
            _dbContext.SaveChanges();

            return like.Id;
        }

        public void Remove(Like like)
        {
            _dbContext.Likes.Remove(like);
            _dbContext.SaveChanges();
        }

        public bool CheckIsPostLikedByUser(Guid postId, Guid userId)
        {
            return _dbContext.Likes
                .Any(i => i.PostId == postId && i.UserId == userId);
        }

        public int GetPostLikesCount(Guid postId)
        {
            return _dbContext.Likes
                .Where(i => i.PostId == postId)
                .Count();
        }

        public Like? GetLikeOnPostByUser(Guid postId, Guid userId)
        {
            return _dbContext.Likes
                .FirstOrDefault(i => i.PostId == postId && i.UserId == userId);
        }
    }
}
