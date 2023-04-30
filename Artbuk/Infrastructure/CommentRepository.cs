using Artbuk.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Artbuk.Infrastructure
{
    public class CommentRepository
    {
        private readonly ArtbukContext _dbContext;

        public CommentRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Comment? GetById(Guid id)
        {
            return _dbContext.Comments
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Comment> GetCommentsByPostId(Guid postId)
        {
            return _dbContext.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedOn)
                .ToList();
        }

        public List<Comment> GetCommentsByUserId(Guid userId)
        {
            return _dbContext.Comments
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedOn)
                .ToList();
        }

        public Guid Add(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();

            return comment.Id;
        }

        public int RemoveCommentsByPostId(Guid postId)
        {
            var comments = GetCommentsByPostId(postId);
            _dbContext.Comments.RemoveRange(comments);

            return _dbContext.SaveChanges();
        }

        public void RemoveCommentsByUserId(Guid userId)
        {
            var comments = GetCommentsByUserId(userId);
            _dbContext.Comments.RemoveRange(comments);

            _dbContext.SaveChanges();
        }

        public int Remove(Comment comment)
        {
            _dbContext.Comments.Remove(comment);
            return _dbContext.SaveChanges();
        }
    }
}
