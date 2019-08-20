using Backend.Domain.Commands.Weather;
using Backend.Domain.Interfaces.Handlers;
using Backend.Domain.Interfaces.Repositories.Read;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class WeatherController : Controller
    {
        private readonly IWeatherReadRepository _repository;
        private readonly IWeatherCommandHandler _handler;

        public WeatherController(IWeatherReadRepository repository,
            IWeatherCommandHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        [HttpGet("weather/getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repository.GetById(id);
            return Ok(result);
        }

        [HttpPost("weather/insert")]
        public async Task<IActionResult> Insert([FromBody]WeatherInsertCommand command)
        {
            await _handler.Insert(command);
            return Ok(command.Notifications);
        }

        [HttpPut("weather/update")]
        public async Task<IActionResult> Update([FromBody]WeatherUpdateCommand command)
        {
            await _handler.Update(command);
            return Ok(command.Notifications);
        }

        [HttpDelete("weather/delete")]
        public async Task<IActionResult> Delete([FromBody]WeatherDeleteCommand command)
        {
            await _handler.Delete(command);
            return Ok(command.Notifications);
        }
    }
}
