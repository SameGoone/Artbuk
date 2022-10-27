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

        public Task AddAsync(PostInGenre postInGenre)
        {
            _dbContext.PostInGenres.Add(postInGenre);
            return _dbContext.SaveChangesAsync();
        }

        public Task<List<Guid>> GetPostIdsByGenreIdAsync(Guid genreId)
        {
            return _dbContext.PostInGenres
                .Where(i => i.GenreId == genreId)
                .Select(i => i.PostId)
                .Distinct()
                .ToListAsync();
        }
    }
}
