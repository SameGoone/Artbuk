using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface IPostInSoftwareRepository
    {
        List<Guid> GetPostIdsBySoftwareId(Guid softwareId);
        PostInSoftware GetPostInSoftwareByPostId(Guid postId);
        void Add(PostInSoftware postInSoftware);
    }
}
