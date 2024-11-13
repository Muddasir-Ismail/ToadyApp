using eTickets.Models;
using eTickets.wwwroot.json.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTickets.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ICinemaService _service;

        public CinemasController(ICinemaService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var allCinemas = _service.GetAll();
            return View(allCinemas);
        }

        // Get: Cinemas/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Logo,Name,Description")] Cinema cinema)
        {
            if (!ModelState.IsValid) return View(cinema);

            _service.Add(cinema);
            return RedirectToAction(nameof(Index));
        }

        // Get: Cinemas/Details/1
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var cinemaDetails = _service.GetById(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        // Get: Cinemas/Edit/1
        public IActionResult Edit(int id)
        {
            var cinemaDetails = _service.GetById(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,Logo,Name,Description")] Cinema cinema)
        {
            if (!ModelState.IsValid) return View(cinema);

            _service.Update(id, cinema);
            return RedirectToAction(nameof(Index));
        }

        // Get: Cinema/Delete/1
        public IActionResult Delete(int id)
        {
            var cinemaDetails = _service.GetById(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            var cinemaDetails = _service.GetById(id);
            if (cinemaDetails == null) return View("NotFound");

            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
