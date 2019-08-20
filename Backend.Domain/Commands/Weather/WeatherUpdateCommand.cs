using FluentValidation;
using FluentValidation.Results;

namespace Backend.Domain.Commands.Weather
{
    public class WeatherUpdateCommand : WeatherBaseCommand
    {
        public InlineValidator<T> GetValidator<T>() where T : WeatherUpdateCommand
        {
            var validator = new InlineValidator<T>();

            validator.RuleFor(p => p.Id)
                .NotEmpty()
                .WithMessage("The field id must have a value.");

            return validator;
        }

        public ValidationResult Validate()
        {
            return GetValidator<WeatherUpdateCommand>().Validate(this);
        }
    }
}
