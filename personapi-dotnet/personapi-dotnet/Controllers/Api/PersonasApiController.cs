using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers.Api
{
    [ApiController]
    [Route("api/personas")]
    public class PersonasApiController : ControllerBase
    {
        private readonly IPersonaRepository _personas;
        public PersonasApiController(IPersonaRepository personas) => _personas = personas;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _personas.GetAll());

        [HttpGet("{cc}")]
        public async Task<IActionResult> GetById(string cc)
        {
            var persona = await _personas.GetById(cc);
            return persona == null ? NotFound() : Ok(persona);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Persona persona)
        {
            if (await _personas.GetById(persona.Cc) != null)
                return Conflict("Ya existe una persona con esa identificación.");
            await _personas.Add(persona);
            return CreatedAtAction(nameof(GetById), new { cc = persona.Cc }, persona);
        }

        [HttpPut("{cc}")]
        public async Task<IActionResult> Update(string cc, Persona persona)
        {
            if (cc != persona.Cc) return BadRequest();
            var actual = await _personas.GetById(cc);
            if (actual == null) return NotFound();
            // Se actualiza el registro ya cargado por EF
            actual.Nombre = persona.Nombre;
            actual.Apellido = persona.Apellido;
            actual.Genero = persona.Genero;
            actual.Edad = persona.Edad;
            await _personas.Update(actual);
            return NoContent();
        }

        [HttpDelete("{cc}")]
        public async Task<IActionResult> Delete(string cc)
        {
            if (await _personas.GetById(cc) == null) return NotFound();
            await _personas.Delete(cc);
            return NoContent();
        }
    }
}
