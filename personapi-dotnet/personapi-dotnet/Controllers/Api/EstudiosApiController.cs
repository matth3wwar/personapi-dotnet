using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers.Api
{
    // Estudio tiene clave compuesta: (idProf, ccPer)
    [ApiController]
    [Route("api/estudios")]
    public class EstudiosApiController : ControllerBase
    {
        private readonly IEstudioRepository _estudios;
        private readonly IPersonaRepository _personas;
        private readonly IProfesionRepository _profesiones;

        public EstudiosApiController(IEstudioRepository estudios, IPersonaRepository personas,
            IProfesionRepository profesiones)
        {
            _estudios = estudios;
            _personas = personas;
            _profesiones = profesiones;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _estudios.GetAll());

        [HttpGet("{idProf}/{ccPer}")]
        public async Task<IActionResult> GetById(int idProf, string ccPer)
        {
            var estudio = await _estudios.GetById(idProf, ccPer);
            return estudio == null ? NotFound() : Ok(estudio);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Estudio estudio)
        {
            if (await _profesiones.GetById(estudio.IdProf) == null)
                return BadRequest("No existe esa profesión.");
            if (await _personas.GetById(estudio.CcPer) == null)
                return BadRequest("No existe una persona con esa identificación.");
            if (await _estudios.GetById(estudio.IdProf, estudio.CcPer) != null)
                return Conflict("Ya existe un registro para esa combinación de profesión y persona.");
            await _estudios.Add(estudio);
            return CreatedAtAction(nameof(GetById), new { idProf = estudio.IdProf, ccPer = estudio.CcPer }, estudio);
        }

        [HttpPut("{idProf}/{ccPer}")]
        public async Task<IActionResult> Update(int idProf, string ccPer, Estudio estudio)
        {
            if (idProf != estudio.IdProf || ccPer != estudio.CcPer) return BadRequest();
            var actual = await _estudios.GetById(idProf, ccPer);
            if (actual == null) return NotFound();
            // Se actualiza el registro ya cargado por EF
            actual.Fecha = estudio.Fecha;
            actual.Univer = estudio.Univer;
            await _estudios.Update(actual);
            return NoContent();
        }

        [HttpDelete("{idProf}/{ccPer}")]
        public async Task<IActionResult> Delete(int idProf, string ccPer)
        {
            if (await _estudios.GetById(idProf, ccPer) == null) return NotFound();
            await _estudios.Delete(idProf, ccPer);
            return NoContent();
        }
    }
}
