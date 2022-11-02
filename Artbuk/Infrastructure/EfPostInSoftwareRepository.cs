using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Infrastructure
{
    public class EfPostInSoftwareRepository : IPostInSoftwareRepository
    {
        private readonly ArtbukContext _dbContext;
        public EfPostInSoftwareRepository(ArtbukContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(PostInSoftware postInSoftware)
        {
            if (postInSoftware != null)
            {
                _dbContext.PostInSoftware.Add(postInSoftware);
                _dbContext.SaveChanges();
            }
        }

        public List<Guid> GetPostIdsBySoftwareId(Guid softwareId)
        {
            return _dbContext.PostInSoftware
                .Where(i => i.SoftwareId == softwareId)
                .Select(i => i.PostId)
                .Distinct()
                .ToList();
        }
    }
}
