using Artbuk.Models;

namespace Artbuk.Infrastructure.ViewData
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
