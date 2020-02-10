using BeezyBackend.API.Models;
using BeezyBackend.Domain.Repositories;
using BeezyBackend.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeezyBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly IMoviesRepository _repository;
        private readonly IWeekDates _weekDates;

        private readonly ILogger<ManagersController> _logger;

        public ManagersController(IMoviesRepository repository, IWeekDates weekDates, ILogger<ManagersController> logger)
        {
            _repository = repository;
            _weekDates = weekDates;
            _logger = logger;
        }

        [HttpGet("UpcomingMovies")]
        [ProducesResponseType(typeof(IEnumerable<V1.RecomendedMovieResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUpcomingMovies([BindRequired, FromQuery] int weeksFromNow, [BindRequired, FromQuery] string ageRate, [BindRequired, FromQuery] string genre)
        {
            _logger.LogInformation("Call to api/Managers/UpcomingMovies");
            return BadRequest();
        }

        [HttpGet("SuggestedBillboard")]
        [ProducesResponseType(typeof(IEnumerable<V1.BillboardResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSuggestedBillboard([BindRequired, FromQuery] int weeksFromNow, [BindRequired, FromQuery] int numberOfScreens, [FromQuery] bool basedOnCityMovies)
        {
            _logger.LogInformation("Call to api/Managers/SuggestedBillboard");
            return BadRequest();
        }

        [HttpGet("IntelligentBillboard")]
        [ProducesResponseType(typeof(IEnumerable<V1.IntelligentBillboardResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIntelligentBillboard([BindRequired, FromQuery] int weeksFromNow,
                                                                 [BindRequired, FromQuery] int numberOfBigScreens,
                                                                 [BindRequired, FromQuery] int numberOfSmallScreens,
                                                                 [FromQuery] bool basedOnCityMovies)
        {
            try
            {
                _logger.LogInformation("Call to api/Managers/IntelligentBillboard");
                var boardCalculator = new BillboardCalculator(_repository, _weekDates, weeksFromNow, numberOfBigScreens, numberOfSmallScreens, basedOnCityMovies);
                var billboard = await boardCalculator.GetBillboard();
                return Ok(billboard.ToDto());
            }
            catch (ArgumentException exception)
            {
                _logger.LogError("Error in api/Managers/IntelligentBillboard: {0}", exception.Message);
                return BadRequest();
            }
        }
    }
}
