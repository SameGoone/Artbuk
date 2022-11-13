using Artbuk.Core.Interfaces;
using Artbuk.Models;

namespace Artbuk.Controllers
{
    public class CreatePostData
    {
        public List<Genre> Genres { get; set; }
        public List<Software> Software { get; set; }

        public CreatePostData(List<Genre> genres, List<Software> softwares)
        {
            Genres = genres;
            Software = softwares;
        }
    }
}
