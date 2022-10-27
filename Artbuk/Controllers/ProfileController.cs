using Artbuk.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Artbuk.Controllers
{
    public class ProfileController : Controller
    {
        IPostRepository _postRepository;
        IGenreRepository _genreRepository;
        IPostInGenreRepository _postInGenreRepository;

        public ProfileController(IPostRepository postRepository, IGenreRepository genreRepository, IPostInGenreRepository postInGenreRepository)
        {
            _postRepository = postRepository;
            _genreRepository = genreRepository;
            _postInGenreRepository = postInGenreRepository;
        }

        public IActionResult DeletePost(Guid? postId)
        {
            if (postId == null)
            {
                return new NoContentResult();
            }

            var post = _postRepository.GetById(postId);
            if (post == null)
            {
                return new NoContentResult();
            }

            _postRepository.Delete(post);
            return RedirectToAction("Feed", "Feed");
        }
    }
}
