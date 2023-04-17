using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class CommentRepository
    {
        private readonly ArtbukContext _dbContext;

        public CommentRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Genre GetById(Guid id)
        {
            return _dbContext.Genres
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

        public void DeleteRange(IEnumerable<Comment> comments)
        {
            foreach(var comment in comments)
            {
                _dbContext.Comments.Remove(comment);
            }

            _dbContext.SaveChanges();
        }
    }
}
