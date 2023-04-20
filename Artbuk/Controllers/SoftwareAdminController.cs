using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Mvc;

namespace Artbuk.Controllers
{
    public class SoftwareAdminController : Controller
    {
        SoftwareRepository _softwareRepository;

        public SoftwareAdminController(SoftwareRepository softwareRepository)
        {
            _softwareRepository = softwareRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_softwareRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Software(Guid softwareId)
        {
            if (softwareId == Guid.Empty)
            {
                return BadRequest();
            }

            return View(_softwareRepository.GetById(softwareId));
        }

        [HttpGet]
        public IActionResult CreateSoftware()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSoftware(string body)
        {
            if (string.IsNullOrEmpty(body))
            {
                return BadRequest();
            }

            _softwareRepository.Add(new Software { Id = Guid.NewGuid(), Name = body });
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(Software software)
        {
            if (software == null)
            {
                return BadRequest();
            }

            _softwareRepository.Update(software);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Software software)
        {
            if (software == null)
            {
                return BadRequest();
            }

            _softwareRepository.Delete(software);
            return RedirectToAction("Index");
        }
    }
}