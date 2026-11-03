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
            // Se cargan también sus estudios para borrarlos junto con la profesión
            var profesion = await _context.Profesions
                .Include(p => p.Estudios)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (profesion != null)
            {
                _context.Estudios.RemoveRange(profesion.Estudios);
                _context.Profesions.Remove(profesion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
