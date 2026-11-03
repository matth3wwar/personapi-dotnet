using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers.Api
{
    [ApiController]
    [Route("api/telefonos")]
    public class TelefonosApiController : ControllerBase
    {
        private readonly ITelefonoRepository _telefonos;
        private readonly IPersonaRepository _personas;

        public TelefonosApiController(ITelefonoRepository telefonos, IPersonaRepository personas)
        {
            _telefonos = telefonos;
            _personas = personas;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _telefonos.GetAll());

        [HttpGet("{num}")]
        public async Task<IActionResult> GetById(string num)
        {
            var telefono = await _telefonos.GetById(num);
            return telefono == null ? NotFound() : Ok(telefono);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Telefono telefono)
        {
            if (await _personas.GetById(telefono.Dueno) == null)
                return BadRequest("No existe una persona con esa identificación.");
            if (await _telefonos.GetById(telefono.Num) != null)
                return Conflict("Ese número ya está registrado.");
            await _telefonos.Add(telefono);
            return CreatedAtAction(nameof(GetById), new { num = telefono.Num }, telefono);
        }

        [HttpPut("{num}")]
        public async Task<IActionResult> Update(string num, Telefono telefono)
        {
            if (num != telefono.Num) return BadRequest();
            var actual = await _telefonos.GetById(num);
            if (actual == null) return NotFound();
            if (await _personas.GetById(telefono.Dueno) == null)
                return BadRequest("No existe una persona con esa identificación.");
            // Se actualiza el registro ya cargado por EF
            actual.Oper = telefono.Oper;
            actual.Dueno = telefono.Dueno;
            await _telefonos.Update(actual);
            return NoContent();
        }

        [HttpDelete("{num}")]
        public async Task<IActionResult> Delete(string num)
        {
            if (await _telefonos.GetById(num) == null) return NotFound();
            await _telefonos.Delete(num);
            return NoContent();
        }
    }
}
