using Backend.Domain.Interfaces.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Tests.Fakes
{
    public class FakeSongsCommandHandler : ISongsCommandHandler
    {
        public Task<List<string>> GetSongsPerCity(string city)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetSongsPerCity(double lat, double lon)
        {
            throw new NotImplementedException();
        }

        public string GetStylePerTemperature(double temperature)
        {
            string type;
            switch (temperature)
            {
                case double temp when (temp >= 30):
                    type = "party";
                    break;
                case double temp when (temp >= 15 && temp < 30):
                    type = "pop";
                    break;
                case double temp when (temp >= 10 && temp < 15):
                    type = "rock";
                    break;
                default:
                    type = "classical";
                    break;
            }

            return type;
        }

        public Task<double> GetTemperature(string city)
        {
            throw new NotImplementedException();
        }

        public Task<double> GetTemperature(double lat, double lon)
        {
            throw new NotImplementedException();
        }
    }
}
