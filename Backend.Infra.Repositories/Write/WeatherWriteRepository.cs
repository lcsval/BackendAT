using Backend.Domain.Entities.Weather;
using Backend.Domain.Interfaces.Infra;
using Backend.Domain.Interfaces.Repositories.Write;
using Dapper;
using System.Threading.Tasks;

namespace Backend.Infra.Repositories.Write
{
    public class WeatherWriteRepository : BaseWriteRepository, IWeatherWriteRepository
    {
        public WeatherWriteRepository(IUnitOfWork uow) : base(uow)
        {

        }

        public async Task Insert(Weather entity)
        {
            await Connection.ExecuteAsync(@"
                INSERT INTO Weather 
                    (main)
                VALUES
                    (@main);", entity);
        }

        public async Task Update(Weather entity)
        {
            await Connection.ExecuteAsync(@"
                UPDATE 
                    Weather
                SET
                    Main = @main
                WHERE
                    Id = @id", entity);
        }

        public async Task Delete(int id)
        {
            await Connection.ExecuteAsync(@"DELETE FROM Weather WHERE Id = @Id", new { Id = id });
        }

    }
}
