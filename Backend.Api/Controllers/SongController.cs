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
using Backend.Domain.Interfaces.Handlers;
using Backend.Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Backend.Api.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class SongController : Controller
    {
        private readonly ISongsCommandHandler _handler;

        public SongController(ISongsCommandHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        [Route("songs/{city}")]
        public async Task<IActionResult> Get(string city)
        {
            try
            {
                return Ok(await _handler.GetSongsPerCity(city));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("songs/{lat}/{lon}")]
        public async Task<IActionResult> Get(double lat, double lon)
        {
            try
            {
                return Ok(await _handler.GetSongsPerCity(lat, lon));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}