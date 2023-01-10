using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class EfCommentRepository : ICommentRepository
    {
        private readonly ArtbukContext _dbContext;

        public EfCommentRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
        }

        public List<Comment> GetCommentsByPostId(Guid postId)
        {
            return _dbContext.Comments
                .Where(i => i.PostId == postId)
                .ToList();
        }
    }
}
