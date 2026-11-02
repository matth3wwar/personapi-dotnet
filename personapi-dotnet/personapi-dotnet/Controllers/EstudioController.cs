using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.DAO.Repositories;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    public class EstudioController : Controller
    {
        private readonly IEstudioRepository _estudios;
        private readonly IPersonaRepository _personas;
        private readonly IProfesionRepository _profesiones;

        public EstudioController(IEstudioRepository estudios, IPersonaRepository personas,
            IProfesionRepository profesiones)
        {
            _estudios = estudios;
            _personas = personas;
            _profesiones = profesiones;
        }

        private async Task CargarListas(int? profesion = null, string? persona = null)
        {
            ViewBag.Profesiones = new SelectList(await _profesiones.GetAll(), "Id", "Nom", profesion);
            ViewBag.Personas = new SelectList(await _personas.GetAll(), "Cc", "Cc", persona);
        }

        public async Task<IActionResult> Index() => View(await _estudios.GetAll());

        public async Task<IActionResult> Details(int? idProf, string? ccPer)
        {
            if (idProf == null || string.IsNullOrWhiteSpace(ccPer)) return NotFound();
            var item = await _estudios.GetById(idProf.Value, ccPer);
            return item == null ? NotFound() : View(item);
        }

        public async Task<IActionResult> Create()
        {
            await CargarListas();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Estudio estudio)
        {
            if (!ModelState.IsValid)
            {
                await CargarListas(estudio.IdProf, estudio.CcPer);
                return View(estudio);
            }
            if (await _estudios.GetById(estudio.IdProf, estudio.CcPer) != null)
            {
                ModelState.AddModelError("", "Ya existe un registro para esa combinación de profesión y persona.");
                await CargarListas(estudio.IdProf, estudio.CcPer);
                return View(estudio);
            }
            await _estudios.Add(estudio);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? idProf, string? ccPer)
        {
            if (idProf == null || string.IsNullOrWhiteSpace(ccPer)) return NotFound();
            var item = await _estudios.GetById(idProf.Value, ccPer);
            if (item == null) return NotFound();
            await CargarListas(item.IdProf, item.CcPer);
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idProf, string ccPer, Estudio estudio)
        {
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer) return BadRequest();
            if (!ModelState.IsValid)
            {
                await CargarListas(estudio.IdProf, estudio.CcPer);
                return View(estudio);
            }
            await _estudios.Update(estudio);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? idProf, string? ccPer)
        {
            if (idProf == null || string.IsNullOrWhiteSpace(ccPer)) return NotFound();
            var item = await _estudios.GetById(idProf.Value, ccPer);
            return item == null ? NotFound() : View(item);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idProf, string ccPer)
        {
            await _estudios.Delete(idProf, ccPer);
            return RedirectToAction(nameof(Index));
        }
    }
}
