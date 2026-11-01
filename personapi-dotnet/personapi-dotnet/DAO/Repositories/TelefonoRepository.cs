using Microsoft.EntityFrameworkCore;
using personapi_dotnet.DAO.Interfaces;
using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAO.Repositories
{
    public class TelefonoRepository : ITelefonoRepository
    {
        private readonly PersonaDbContext _context;

        public TelefonoRepository(PersonaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Telefono>> GetAll()
        {
            return await _context.Telefonos.ToListAsync();
        }

        public async Task<Telefono?> GetById(string cc)
        {
            return await _context.Telefonos.FindAsync(cc);
        }

        public async Task Add(Telefono persona)
        {
            _context.Telefonos.Add(persona);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Telefono persona)
        {
            _context.Telefonos.Update(persona);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string cc)
        {
            var persona = await GetById(cc);

            if (persona != null)
            {
                _context.Telefonos.Remove(persona);
                await _context.SaveChangesAsync();
            }
        }
    }
}
