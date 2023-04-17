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

        public Comment GetById(Guid id)
        {
            return _dbContext.Comments
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Comment> GetComments(Guid? postId)
        {
            return _dbContext.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .OrderByDescending(c => c.CreatedOn)
                .ToList();
        }

        public void Add(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
        }

        public void DeleteCommentsByPostId(Guid? postId)
        {
            var comments = GetComments(postId);

            if (comments.Count > 0)
            {
                _dbContext.Comments.RemoveRange(comments);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(Comment comment)
        {
            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
        }
    }
}
