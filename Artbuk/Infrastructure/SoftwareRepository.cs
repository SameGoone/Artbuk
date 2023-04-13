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

        public Software GetById(Guid id)
        {
            return _dbContext.Software
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Software> GetAll()
        {
            return _dbContext.Software
                .ToList();
        }

        public void Add(Software software)
        {
            _dbContext.Software.Add(software);
            _dbContext.SaveChanges();
        }

        public void Update(Software software)
        {
            _dbContext.Entry(software).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
