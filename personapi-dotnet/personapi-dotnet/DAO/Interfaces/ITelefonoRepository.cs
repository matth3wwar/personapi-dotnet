using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAO.Interfaces
{
    public interface ITelefonoRepository
    {
        Task<IEnumerable<Telefono>> GetAll();

        Task<Telefono?> GetById(string cc);

        Task Add(Telefono telefono);

        Task Update(Telefono telefono);

        Task Delete(string cc);
    }
}
