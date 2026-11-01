using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.Controllers
{
    [Route("api/personas")]
    [ApiController]
    public class PersonaApiController : Controller
    {
        private readonly IPersonaRepository _repository;

        public PersonaApiController(IPersonaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repository.GetAll());
        }

        [HttpGet("{cc}")]
        public async Task<IActionResult> GetById(string cc)
        {
            var persona = await _repository.GetById(cc);

            if (persona == null)
                return NotFound();

            return Ok(persona);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Persona persona)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _repository.GetById(persona.Cc) != null)
                return Conflict("La persona ya existe.");

            await _repository.Add(persona);

            return CreatedAtAction(
                nameof(GetById),
                new { cc = persona.Cc },
                persona
            );
        }

        [HttpPut("{cc}")]
        public async Task<IActionResult> Update(
            string cc, Persona persona)
        {
            if (cc != persona.Cc)
                return BadRequest();

            var existing = await _repository.GetById(cc);

            if (existing == null)
                return NotFound();

            await _repository.Update(persona);

            return NoContent();
        }

        [HttpDelete("{cc}")]
        public async Task<IActionResult> Delete(string cc)
        {
            var persona = await _repository.GetById(cc);

            if (persona == null)
                return NotFound();

            await _repository.Delete(cc);

            return NoContent();
        }
    }
}