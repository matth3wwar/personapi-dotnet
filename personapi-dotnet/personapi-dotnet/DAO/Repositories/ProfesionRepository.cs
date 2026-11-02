using Microsoft.EntityFrameworkCore;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAO.Repositories
{
    public class ProfesionRepository : IProfesionRepository
    {
        private readonly PersonaDbContext _context;

        public ProfesionRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profesion>> GetAll()
        {
            return await _context.Profesions.ToListAsync();
        }

        public async Task<Profesion?> GetById(int id)
        {
            return await _context.Profesions.FindAsync(id);
        }

        public async Task Add(Profesion persona)
        {
            _context.Profesions.Add(persona);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Profesion persona)
        {
            _context.Profesions.Update(persona);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var persona = await GetById(id);

            if (persona != null)
            {
                _context.Profesions.Remove(persona);
                await _context.SaveChangesAsync();
            }
        }
    }
}
