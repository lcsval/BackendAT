using Backend.Domain.Entities.Weather;
using Backend.Domain.Interfaces.Repositories.Read;
using Dapper;
using System.Threading.Tasks;

namespace Backend.Infra.Repositories.Read
{
    public class WeatherReadRepository :  BaseReadRepository, IWeatherReadRepository
    {
        public WeatherReadRepository()
        {

        }

        public async Task<Weather> GetById(int id)
        {
            using (var connection = GetConnection())
            {
                return await connection.QueryFirstAsync<Weather>(@"
                    SELECT
                        *
                    FROM
                        weather
                    WHERE
                        id = @Id", new { Id = id });
            }
        }
    }
}
