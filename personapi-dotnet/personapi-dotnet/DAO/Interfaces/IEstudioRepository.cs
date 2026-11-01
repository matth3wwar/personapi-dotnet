using personapi_dotnet.Models.Entities;

namespace personapi_dotnet.DAO.Interfaces
{
    public interface IEstudioRepository
    {
        Task<IEnumerable<Estudio>> GetAll();

        Task<Estudio?> GetById(string cc);

        Task Add(Estudio estudio);

        Task Update(Estudio estudio);

        Task Delete(string cc);
    }
}
