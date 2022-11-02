using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class EfSoftwareRepository : ISoftwareRepository
    {
        private readonly ArtbukContext _dbContext;

        public EfSoftwareRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Software GetById(Guid id)
        {
            return _dbContext.Software
                .FirstOrDefault(i => i.Id == id);
        }

        public List<Software> List()
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
