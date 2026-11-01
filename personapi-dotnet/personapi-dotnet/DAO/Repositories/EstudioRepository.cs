using Microsoft.EntityFrameworkCore;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAO.Repositories
{
    public class EstudioRepository : IEstudioRepository
    {
        private readonly PersonaDbContext _context;

        public EstudioRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estudio>> GetAll()
        {
            return await _context.Estudios.ToListAsync();
        }

        public async Task<Estudio?> GetById(string cc)
        {
            return await _context.Estudios.FindAsync(cc);
        }

        public async Task Add(Estudio persona)
        {
            _context.Estudios.Add(persona);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Estudio persona)
        {
            _context.Estudios.Update(persona);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string cc)
        {
            var persona = await GetById(cc);

            if (persona != null)
            {
                _context.Estudios.Remove(persona);
                await _context.SaveChangesAsync();
            }
        }
    }
}
