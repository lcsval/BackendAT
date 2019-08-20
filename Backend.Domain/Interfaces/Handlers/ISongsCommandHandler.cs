using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Domain.Interfaces.Handlers
{
    public interface ISongsCommandHandler
    {
        Task<List<string>> GetSongsPerCity(string city);
        Task<List<string>> GetSongsPerCity(double lat, double lon);
        string GetStylePerTemperature(double temperature);

        Task<double> GetTemperature(string city);
        Task<double> GetTemperature(double lat, double lon);
    }
}
