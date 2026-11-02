using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class ProfesionController : Controller
    {
        private readonly IProfesionRepository _profesiones;
        public ProfesionController(IProfesionRepository profesiones) => _profesiones = profesiones;

        public async Task<IActionResult> Index() => View(await _profesiones.GetAll());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _profesiones.GetById(id.Value);
            return item == null ? NotFound() : View(item);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Profesion profesion)
        {
            if (!ModelState.IsValid) return View(profesion);
            await _profesiones.Add(profesion);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _profesiones.GetById(id.Value);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profesion profesion)
        {
            if (id != profesion.Id) return BadRequest();
            if (!ModelState.IsValid) return View(profesion);
            await _profesiones.Update(profesion);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var item = await _profesiones.GetById(id.Value);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _profesiones.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
