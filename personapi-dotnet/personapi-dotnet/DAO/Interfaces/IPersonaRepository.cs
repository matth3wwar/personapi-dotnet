using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAO.Interfaces
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> GetAll();

        Task<Persona?> GetById(string cc);

        Task Add(Persona persona);

        Task Update(Persona persona);

        Task Delete(string cc);
    }
}