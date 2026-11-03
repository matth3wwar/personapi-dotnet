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
            // Se cargan también sus teléfonos y estudios para borrarlos junto con la persona
            var persona = await _context.Personas
                .Include(p => p.Telefonos)
                .Include(p => p.Estudios)
                .FirstOrDefaultAsync(p => p.Cc == cc);

            if (persona != null)
            {
                _context.Telefonos.RemoveRange(persona.Telefonos);
                _context.Estudios.RemoveRange(persona.Estudios);
                _context.Personas.Remove(persona);
                await _context.SaveChangesAsync();
            }
        }
    }
}