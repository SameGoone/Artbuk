using System.Collections.Generic;
using System.Threading.Tasks;
using Artbuk.Models;

namespace Artbuk.Core.Interfaces
{
    public interface ICommentRepository
    {
        void Add(Comment comment);

        List<Comment> GetCommentsByPostId(Guid postId);
    }
}
