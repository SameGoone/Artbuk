using Artbuk.Models;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Infrastructure
{
    public class SoftwareRepository
    {
        private readonly ArtbukContext _dbContext;

        public SoftwareRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Software? GetById(Guid id)
        {
            return _dbContext.Software
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Software> GetAll()
        {
            return _dbContext.Software
                .ToList();
        }

        public Guid Add(Software software)
        {
            _dbContext.Software.Add(software);
            _dbContext.SaveChanges();

            return software.Id;
        }

        public int Update(Software software)
        {
            _dbContext.Entry(software).State = EntityState.Modified;
            return _dbContext.SaveChanges();
        }

        public int Remove(Software software)
        {
            _dbContext.Remove(software);
            return _dbContext.SaveChanges();
        }
    }
}
