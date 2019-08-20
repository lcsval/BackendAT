using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain.Entities.Spotify;
using Backend.Domain.Entities.Token;
using Backend.Domain.Entities.TracksSpotify;
using Backend.Domain.Entities.Weather;
using Backend.Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Backend.Api.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class BackendController : Controller
    {

        [HttpGet]
        [Route("weather/{city}")]
        public async Task<IActionResult> Get(string city)
        {
            var url = "http://api.openweathermap.org/data/2.5/weather?q=" + $"{city}" + "&units=metric&APPID=" + $"{Settings.OpenWeatherKey}" + "&units=celsius";

            var rootWeather = new RootWeather();
            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    rootWeather = JsonConvert.DeserializeObject<RootWeather>(apiResponse);
                    return Json(rootWeather);

                }
            }
        }

        [HttpGet]
        [Route("weather/spotify")]
        public async Task<IActionResult> Get()
        {
            var token = this.GetClientCredentialsAuthToken();


            var type = "party";
            //var type = "pop";
            //var type = "rock";
            //var type = "classical";

            var urlSpotify = "https://api.spotify.com/v1/browse/categories/" + $"{type}" + "/playlists";
            var rootSpotify = new RootSpotify();
            var rootTracks = new RootTracks();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
            using (var response = await client.GetAsync(urlSpotify))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                rootSpotify = JsonConvert.DeserializeObject<RootSpotify>(apiResponse);

                var client2 = new HttpClient();
                client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
                using (var resp = await client2.GetAsync(rootSpotify.playlists.items[0].tracks.href))
                {
                    string apirespjson = await resp.Content.ReadAsStringAsync();

                    rootTracks = JsonConvert.DeserializeObject<RootTracks>(apirespjson);
                }

                var c = rootTracks.items.Select(s => s.track.name);

                return Json(c);
            }
        }

        // [HttpGet]
        // [Route("weather/{id}")]
        // public IActionResult GetById(int id)
        // {
        //     return Ok("id");
        // }

        // [HttpPost]
        // [Route("weather")]
        // public Task<ICommandResult> Post([FromBody]CreateCommand command)
        // {
        //     return _handler.Handle(command);
        // }

        // [HttpPut]
        // [Route("weather/{id}")]
        // public Task<IActonResult> Put([FromBody]CreateCommand command) //Criar command
        // {
        //     return OK("ok");
        // }

        // [HttpDelete]
        // [Route("backend/{id}")]
        // public object Delete(int id)
        // {
        //     return new { message = removido com sucesso!" };
        // }


        public TokenObject GetClientCredentialsAuthToken()
        {
            //var spotifyAccessToken = "BQBchvB7sphC4gPGWpptbYKNyxlwm-9aGEn4dHbFR5ojP6NYy8D-6VQoUzRI99U_TWqea5VLkcDU9jKD8ryEsBXqMw37pENyJvNukmeecKV5utm-HFDRlCHT8nYaprV3GMsaxg9Kbq392XWziotVqxh2SDUIukj4NSQe";
            //var spotifyRefreshToken = "AQD76DLZ8UWp6FE3JVRiiZjA3639zrCj64w1rijVvW7N0WDbnnCqWrbQNMTOotHlHGFt_D9PXpTaMVwAGy0t_S5Ft08vASJSHSJv2qEgtRAmJ4F47cayoorUhQeVTonwxJTR1A";

            //var spotifyClient = "40c785d5a33241108b770f70487c2bf8";
            //var spotifySecret = "fc3dfeb1e02e41efa5e5d41c8faadc3a";

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