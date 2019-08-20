using Backend.Domain.Entities.Weather;
using System.Threading.Tasks;

namespace Backend.Domain.Interfaces.Repositories.Write
{
    public interface IWeatherWriteRepository
    {
        Task Insert(Weather entity);
        Task Update(Weather entity);
        Task Delete(int id);
    }
}
