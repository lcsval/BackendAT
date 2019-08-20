using Backend.Domain.Entities.Weather;
using System.Threading.Tasks;

namespace Backend.Domain.Interfaces.Repositories.Read
{
    public interface IWeatherReadRepository
    {
        Task<Weather> GetById(int id);
    }
}
