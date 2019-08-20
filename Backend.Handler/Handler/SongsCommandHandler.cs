using Backend.Domain.Entities.Spotify;
using Backend.Domain.Entities.Token;
using Backend.Domain.Entities.TracksSpotify;
using Backend.Domain.Entities.Weather;
using Backend.Domain.Interfaces.Handlers;
using Backend.Domain.Interfaces.Infra;
using Backend.Infra.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Handler.Handler
{
    public class SongsCommandHandler : BaseHandler, ISongsCommandHandler
    {
        public SongsCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<List<string>> GetSongsPerCity(string city)
        {
            var temperature = await GetTemperature(city);
            var style = GetStylePerTemperature(temperature);
            return await GetSongsPerStyle(style);
        }

        public async Task<List<string>> GetSongsPerCity(double lat, double lon)
        {
            var temperature = await GetTemperature(lat, lon);
            var style = GetStylePerTemperature(temperature);
            return await GetSongsPerStyle(style);
        }

        private static string GetStylePerTemperature(double temperature)
        {
            string type;
            switch (temperature)
            {
                case double t when (t >= 30):
                    type = "party";
                    break;
                case double t when (t >= 15 && t < 30):
                    type = "pop";
                    break;
                case double t when (t >= 10 && t < 15):
                    type = "rock";
                    break;
                default:
                    type = "classical";
                    break;
            }

            return type;
        }

        private async Task<double> GetTemperature(string city)
        {
            var url = "http://api.openweathermap.org/data/2.5/weather?q=" +
                $"{city}" +
                "&units=metric&APPID=" +
                $"{Settings.OpenWeatherKey}" +
                "&units=celsius";

            var rootWeather = new RootWeather();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    rootWeather = JsonConvert.DeserializeObject<RootWeather>(apiResponse);
                    return rootWeather.main.temp;
                }
            }
        }

        private async Task<double> GetTemperature(double lat, double lon)
        {
            var url = "http://api.openweathermap.org/data/2.5/weather?" +
                $"lat={lat.ToString().Replace(",", ".")}" +
                $"&lon={lon.ToString().Replace(",", ".")}" +
                "&units=metric&APPID=" +
                $"{Settings.OpenWeatherKey}" +
                "&units=celsius";

            var rootWeather = new RootWeather();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    rootWeather = JsonConvert.DeserializeObject<RootWeather>(apiResponse);
                    return rootWeather.main.temp;
                }
            }
        }

        private async Task<List<string>> GetSongsPerStyle(string style)
        {
            var token = this.GetClientCredentialsAuthToken();
            var urlSpotify = "https://api.spotify.com/v1/browse/categories/" + $"{style}" + "/playlists";
            var rootSpotify = new RootSpotify();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
            using (var response = await client.GetAsync(urlSpotify))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                rootSpotify = JsonConvert.DeserializeObject<RootSpotify>(apiResponse);
                return await GetTracksName(rootSpotify.playlists.items[0].tracks.href, token);
            }
        }

        private async Task<List<string>> GetTracksName(string tracksHREF, TokenObject token)
        {
            var rootTracks = new RootTracks();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
            using (var resp = await client.GetAsync(tracksHREF))
            {
                string apirespjson = await resp.Content.ReadAsStringAsync();
                rootTracks = JsonConvert.DeserializeObject<RootTracks>(apirespjson);
                return rootTracks.items.Select(s => s.track.name).ToList();
            }
        }

        private TokenObject GetClientCredentialsAuthToken()
        {
            var webClient = new WebClient();

            var postparams = new NameValueCollection();
            postparams.Add("grant_type", "client_credentials");

            var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes($"{Settings.SpotifyClientId}:{Settings.SpotifyClientSecret}"));
            webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

            var tokenResponse = webClient.UploadValues("https://accounts.spotify.com/api/token", postparams);
            var textResponse = Encoding.UTF8.GetString(tokenResponse);

            var token = new TokenObject();
            token = JsonConvert.DeserializeObject<TokenObject>(textResponse);
            return token;
        }
    }
}
