using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAO.Interfaces
{
    public interface IProfesionRepository
    {
        Task<IEnumerable<Profesion>> GetAll();

        Task<Profesion?> GetById(string cc);

        Task Add(Profesion profesion);

        Task Update(Profesion profesion);

        Task Delete(string cc);
    }
}
