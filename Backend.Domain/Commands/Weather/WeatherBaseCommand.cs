namespace Backend.Domain.Commands.Weather
{
    public class WeatherBaseCommand : BaseCommand
    {
        public int Id { get; set; }

        public Backend.Domain.Entities.Weather.Weather Map()
        {
            var entity = new Backend.Domain.Entities.Weather.Weather();
            entity.id = this.Id;
            return entity;
        }
    }
}