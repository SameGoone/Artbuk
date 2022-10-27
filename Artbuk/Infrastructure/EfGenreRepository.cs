using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class EfGenreRepository : IGenreRepository
    {
        private readonly ArtbukContext _dbContext;

        public EfGenreRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Genre> GetByIdAsync(Guid id)
        {
            return _dbContext.Genres
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<List<Genre>> ListAsync()
        {
            return _dbContext.Genres
                .ToListAsync();
        }

        public Task AddAsync(Genre genre)
        {
            _dbContext.Genres.Add(genre);
            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(Genre genre)
        {
            _dbContext.Entry(genre).State = EntityState.Modified;
            return _dbContext.SaveChangesAsync();
        }
    }
}
