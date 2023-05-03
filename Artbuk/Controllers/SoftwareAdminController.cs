using Artbuk.Infrastructure;
using Artbuk.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Artbuk.Controllers
{
    [Authorize(Roles = Constants.RoleNames.Admin)]
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
        public IActionResult Software(Guid? softwareId)
        {
            if (softwareId == null)
            {
                return BadRequest();
            }

            return View(_softwareRepository.GetById(softwareId.Value));
        }

        [HttpGet]
        public IActionResult CreateSoftware()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSoftware(string? body)
        {
            if (string.IsNullOrEmpty(body))
            {
                return BadRequest();
            }

            _softwareRepository.Add(new Software { Id = Guid.NewGuid(), Name = body });
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(Software? software)
        {
            if (software == null)
            {
                return BadRequest();
            }

            _softwareRepository.Update(software);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Software? software)
        {
            if (software == null)
            {
                return BadRequest();
            }

            _softwareRepository.Remove(software);
            return RedirectToAction("Index");
        }
    }
}