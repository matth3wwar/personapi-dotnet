using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class TelefonoController : Controller
    {
        private readonly ITelefonoRepository _telefonos;
        private readonly IPersonaRepository _personas;

        public TelefonoController(ITelefonoRepository telefonos, IPersonaRepository personas)
        {
            _telefonos = telefonos;
            _personas = personas;
        }

        private async Task CargarPersonas(string? seleccionado = null)
        {
            var personas = await _personas.GetAll();
            ViewBag.Personas = new SelectList(personas, "Cc", "Cc", seleccionado);
        }

        public async Task<IActionResult> Index() => View(await _telefonos.GetAll());

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var item = await _telefonos.GetById(id);
            return item == null ? NotFound() : View(item);
        }

        public async Task<IActionResult> Create()
        {
            await CargarPersonas();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Telefono telefono)
        {
            if (!ModelState.IsValid)
            {
                await CargarPersonas(telefono.Dueno);
                return View(telefono);
            }
            if (await _telefonos.GetById(telefono.Num) != null)
            {
                ModelState.AddModelError(nameof(telefono.Num), "Ese número ya está registrado.");
                await CargarPersonas(telefono.Dueno);
                return View(telefono);
            }
            await _telefonos.Add(telefono);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var item = await _telefonos.GetById(id);
            if (item == null) return NotFound();
            await CargarPersonas(item.Dueno);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Telefono telefono)
        {
            if (id != telefono.Num) return BadRequest();
            if (!ModelState.IsValid)
            {
                await CargarPersonas(telefono.Dueno);
                return View(telefono);
            }
            await _telefonos.Update(telefono);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var item = await _telefonos.GetById(id);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _telefonos.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
