using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface ISoftwareRepository
    {
        Software GetById(Guid id);
        List<Software> List();
        void Add(Software software);
        void Update(Software software);
    }
}
