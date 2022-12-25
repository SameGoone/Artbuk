using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class EfPostInGenreRepository : IPostInGenreRepository
    {
        private readonly ArtbukContext _dbContext;

        public EfPostInGenreRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(PostInGenre postInGenre)
        {
            _dbContext.PostInGenres.Add(postInGenre);
            _dbContext.SaveChanges();
        }

        public List<Guid> GetPostIdsByGenreId(Guid genreId)
        {
            return _dbContext.PostInGenres
                .Where(i => i.GenreId == genreId)
                .Select(i => i.PostId)
                .Distinct()
                .ToList();
        }

        public PostInGenre GetPostInGenreByPostId(Guid postId)
        {
            return _dbContext.PostInGenres
                .FirstOrDefault(i => i.PostId == postId);
        }
    }
}
