using Artbuk.Models;

namespace Artbuk.Infrastructure.ViewData
{
    public class CreateEditPostData
    {
        public Post Post { get; set; }
        public Genre CurrentGenre { get; set; }
        public Software CurrentSoftware { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Software> Software { get; set; }

        public CreateEditPostData() { }

        public CreateEditPostData(List<Genre> genres, List<Software> softwares)
        {
            Genres = genres;
            Software = softwares;
        }
    }
}
