using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class PersonaController : Controller
    {
        private readonly IPersonaRepository _personas;
        public PersonaController(IPersonaRepository personas) => _personas = personas;

        public async Task<IActionResult> Index() =>
            View(await _personas.GetAll());

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var persona = await _personas.GetById(id);
            return persona == null ? NotFound() : View(persona);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Persona persona)
        {
            if (!ModelState.IsValid) return View(persona);
            if (await _personas.GetById(persona.Cc) != null)
            {
                ModelState.AddModelError(nameof(persona.Cc), "Ya existe una persona con esa identificación.");
                return View(persona);
            }
            await _personas.Add(persona);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var persona = await _personas.GetById(id);
            return persona == null ? NotFound() : View(persona);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Persona persona)
        {
            if (id != persona.Cc) return BadRequest();
            if (!ModelState.IsValid) return View(persona);
            await _personas.Update(persona);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var persona = await _personas.GetById(id);
            return persona == null ? NotFound() : View(persona);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _personas.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
