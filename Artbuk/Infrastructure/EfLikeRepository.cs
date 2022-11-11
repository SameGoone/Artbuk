using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artbuk.Core.Interfaces;
using Artbuk.Models;
using Microsoft.Extensions.Hosting;

namespace Artbuk.Infrastructure
{
    public class EfLikeRepository : ILikeRepository
    {
        private readonly ArtbukContext _dbContext;
        public EfLikeRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Like GetById(Guid likeId)
        {
            return _dbContext.Likes
                .FirstOrDefault(i => i.Id == likeId);
        }

        public List<Like> GetListByUserId(Guid userId)
        {
            return _dbContext.Likes
                .Where(i => i.UserId == userId)
                .ToList();
        }

        public void Add(Like like)
        {
            _dbContext.Likes.Add(like);
            _dbContext.SaveChanges();
        }

        public void Delete(Like like)
        {
            _dbContext.Likes.Remove(like);
            _dbContext.SaveChanges();
        }

        public Like CheckIsLiked(Guid postId, Guid userId)
        {
            return _dbContext.Likes
                .FirstOrDefault(i => i.PostId == postId && i.UserId == userId);
        }

    }
}
