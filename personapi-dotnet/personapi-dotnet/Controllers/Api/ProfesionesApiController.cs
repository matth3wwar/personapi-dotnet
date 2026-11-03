using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers.Api
{
    [ApiController]
    [Route("api/profesiones")]
    public class ProfesionesApiController : ControllerBase
    {
        private readonly IProfesionRepository _profesiones;
        public ProfesionesApiController(IProfesionRepository profesiones) => _profesiones = profesiones;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _profesiones.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesion = await _profesiones.GetById(id);
            return profesion == null ? NotFound() : Ok(profesion);
        }

        // El id lo genera la base de datos (IDENTITY)
        [HttpPost]
        public async Task<IActionResult> Create(Profesion profesion)
        {
            profesion.Id = 0;
            await _profesiones.Add(profesion);
            return CreatedAtAction(nameof(GetById), new { id = profesion.Id }, profesion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Profesion profesion)
        {
            if (id != profesion.Id) return BadRequest();
            var actual = await _profesiones.GetById(id);
            if (actual == null) return NotFound();
            // Se actualiza el registro ya cargado por EF
            actual.Nom = profesion.Nom;
            actual.Des = profesion.Des;
            await _profesiones.Update(actual);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _profesiones.GetById(id) == null) return NotFound();
            await _profesiones.Delete(id);
            return NoContent();
        }
    }
}
