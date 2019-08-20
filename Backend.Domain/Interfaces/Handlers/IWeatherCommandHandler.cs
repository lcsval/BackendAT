using Backend.Domain.Commands.Weather;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Domain.Interfaces.Handlers
{
    public interface IWeatherCommandHandler
    {
        Task<Dictionary<string, string>> Insert(WeatherInsertCommand command);
        Task<Dictionary<string, string>> Update(WeatherUpdateCommand command);
        Task<Dictionary<string, string>> Delete(WeatherDeleteCommand command);
    }
}
