using Microsoft.EntityFrameworkCore;
using personapi_dotnet.Models.Entities;
using personapi_dotnet.DAO.Interfaces;

namespace personapi_dotnet.DAO.Repositories
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly PersonaDbContext _context;

        public PersonaRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Persona>> GetAll()
        {
            return await _context.Personas.ToListAsync();
        }

        public async Task<Persona?> GetById(string cc)
        {
            return await _context.Personas.FindAsync(cc);
        }

        public async Task Add(Persona persona)
        {
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Persona persona)
        {
            _context.Personas.Update(persona);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string cc)
        {
            var persona = await GetById(cc);

            if (persona != null)
            {
                _context.Personas.Remove(persona);
                await _context.SaveChangesAsync();
            }
        }
    }
}