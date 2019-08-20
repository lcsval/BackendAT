using Backend.Domain.Commands.Weather;
using Backend.Domain.Interfaces.Handlers;
using Backend.Domain.Interfaces.Infra;
using Backend.Domain.Interfaces.Repositories.Write;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Handler.Handler
{
    public class WeatherCommandHandler : BaseHandler, IWeatherCommandHandler
    {
        readonly IWeatherWriteRepository _weatherWriteRepository;

        public WeatherCommandHandler(IUnitOfWork uow,
            IWeatherWriteRepository weatherWriteRepository)
            : base(uow)
        {
            _weatherWriteRepository = weatherWriteRepository;
        }

        public async Task<Dictionary<string, string>> Insert(WeatherInsertCommand command)
        {
            try
            {
                var data = command.Validate();
                if (!data.IsValid)
                {
                    HandleErrors(command, data);
                    return command.Notifications;
                }

                await Transaction(command, async () =>
                {
                    await _weatherWriteRepository.Insert(command.Map());
                });

                return command.Notifications;
            }
            catch (Exception ex)
            {
                command.Notifications.Add("Error", ex.Message);
                return command.Notifications;
            }
        }

        public async Task<Dictionary<string, string>> Update(WeatherUpdateCommand command)
        {

            try
            {
                var data = command.Validate();
                if (!data.IsValid)
                {
                    HandleErrors(command, data);
                    return command.Notifications;
                }

                await Transaction(command, async () =>
                {
                    await _weatherWriteRepository.Update(command.Map());
                });

                return command.Notifications;
            }
            catch (Exception ex)
            {
                command.Notifications.Add("Error", ex.Message);
                return command.Notifications;
            }
        }

        public async Task<Dictionary<string, string>> Delete(WeatherDeleteCommand command)
        {

            try
            {
                var data = command.Validate();
                if (!data.IsValid)
                {
                    HandleErrors(command, data);
                    return command.Notifications;
                }

                await Transaction(command, async () =>
                {
                    await _weatherWriteRepository.Delete(command.Id);
                });

                return command.Notifications;
            }
            catch (Exception ex)
            {
                command.Notifications.Add("Error", ex.Message);
                return command.Notifications;
            }
        }

        private static void HandleErrors(WeatherBaseCommand command, FluentValidation.Results.ValidationResult data)
        {
            foreach (var item in data.Errors)
                command.Notifications.Add(item.ErrorCode, item.ErrorMessage);
        }
    }
}
