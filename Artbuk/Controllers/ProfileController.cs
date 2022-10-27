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

        public async Task<IActionResult> DeletePostAsync(Guid? postId)
        {
            if (postId == null)
            {
                return new NoContentResult();
            }

            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                return new NoContentResult();
            }

            await _postRepository.DeleteAsync(post);
            return RedirectToAction("Feed", "Feed");
        }
    }
}
