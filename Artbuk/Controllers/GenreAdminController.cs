using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    [Authorize(Roles = Constants.RoleNames.Admin)]
    public class GenreAdminController : Controller
    {
        GenreRepository _genreRepository;

        public GenreAdminController(GenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_genreRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Genre(Guid genreId) 
        {
            if (genreId == Guid.Empty)
            {
                return BadRequest();
            }

            return View(_genreRepository.GetById(genreId));
        }

        [HttpGet]
        public IActionResult CreateGenre()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateGenre(string body)
        {
            if (string.IsNullOrEmpty(body))
            {
                return BadRequest();
            }

            _genreRepository.Add(new Genre { Id = Guid.NewGuid(), Name = body });
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(Genre genre)
        {
            if (genre == null)
            {
                return BadRequest();
            }

            _genreRepository.Update(genre);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Genre genre)
        {
            if (genre == null)
            {
                return BadRequest();
            }

            _genreRepository.Delete(genre);
            return RedirectToAction("Index");
        }
    }
}
