using BeezyBackend.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeezyBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewersController : ControllerBase
    {

        private readonly ILogger<ViewersController> _logger;

        public ViewersController(ILogger<ViewersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("AllTimeMovies")]
        [ProducesResponseType(typeof(IEnumerable<V1.RecomendedMovieResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTimeMovies([FromQuery] List<string> keywords, [FromQuery] List<string> genres)
        {
            _logger.LogInformation("Call to api/Viewers/AllTimeMovies");
            return BadRequest();
        }

        [HttpGet("UpcomingMovies")]
        [ProducesResponseType(typeof(IEnumerable<V1.RecomendedMovieResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUpcomingMovies([BindRequired, FromQuery] int weeksFromNow, [FromQuery] List<string> keywords, [FromQuery] List<string> genres)
        {
            _logger.LogInformation("Call to api/Viewers/UpcomingMovies");
            return BadRequest();
        }

        [HttpGet("AllTimeTVShows")]
        [ProducesResponseType(typeof(IEnumerable<V1.RecomendedTVShowResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTimeTVShows([FromQuery] List<string> keywords, [FromQuery] List<string> genres)
        {
            _logger.LogInformation("Call to api/Viewers/AllTimeTVShows");
            return BadRequest();
        }

        [HttpGet("AllTimeDocumentaries")]
        [ProducesResponseType(typeof(IEnumerable<V1.RecomendedDocumentaryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTimeDocumentaries([BindRequired, FromQuery] List<string> topics)
        {
            _logger.LogInformation("Call to api/Viewers/AllTimeDocumentaries");
            return BadRequest();
        }
    }
}
