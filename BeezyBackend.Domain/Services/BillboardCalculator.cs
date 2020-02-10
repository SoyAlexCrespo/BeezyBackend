using BeezyBackend.Domain.Models;
using BeezyBackend.Domain.Services.QueryModels;
using BeezyBackend.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BeezyBackend.Domain.Services
{
    public class BillboardCalculator
    {
        private readonly IMoviesRepository _repository;
        private readonly IWeekDates _weekDates;
        private readonly int _weeksFromNow;
        private readonly int _bigRoomsScreens;
        private readonly int _smallRoomsScreens;
        private readonly bool _basedOnCityMovies;

        const int MIN_WEEKS_FROM_NOW = 1;
        const int MAX_WEEKS_FROM_NOW = 52;
        const int MIN_SCREENS = 0;
        const int MAX_SCREENS = 30;        

        public BillboardCalculator(IMoviesRepository repository, IWeekDates weekDates, int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens, bool basedOnCityMovies)
        {
            _repository = repository;
            _weekDates = weekDates;
            _weeksFromNow = weeksFromNow;
            _bigRoomsScreens = bigRoomsScreens;
            _smallRoomsScreens = smallRoomsScreens;
            _basedOnCityMovies = basedOnCityMovies;

            if (!IsValidWeeksFromNow()) throw new ArgumentException($"Weeks must be between {MIN_WEEKS_FROM_NOW} and {MAX_WEEKS_FROM_NOW}");
            if (!IsValidBigRoomsScreensNumber()) throw new ArgumentException($"Number of big rooms must be between {MIN_SCREENS} and {MAX_SCREENS}");
            if (!IsValidSmallRoomsScreensNumber()) throw new ArgumentException($"Number of small rooms must be between {MIN_SCREENS} and {MAX_SCREENS}");
            if (!IsValidTotalRoomsScreensNumber()) throw new ArgumentException($"Total number of rooms must be greater than {MIN_SCREENS}");

        }

        public async Task<Billboard> GetBillboard()
        {
            var moviesInfo = await _repository.GetMoviesInfo(_basedOnCityMovies);            
            return new Billboard(moviesInfo, _weekDates, _weeksFromNow, _bigRoomsScreens, _smallRoomsScreens );
        }

        private bool IsValidWeeksFromNow() => _weeksFromNow >= MIN_WEEKS_FROM_NOW && _weeksFromNow <= MAX_WEEKS_FROM_NOW;
        private bool IsValidBigRoomsScreensNumber() => _bigRoomsScreens >= MIN_SCREENS && _bigRoomsScreens <= MAX_SCREENS;
        private bool IsValidSmallRoomsScreensNumber() => _smallRoomsScreens >= MIN_SCREENS && _smallRoomsScreens <= MAX_SCREENS;
        private bool IsValidTotalRoomsScreensNumber() => _bigRoomsScreens + _smallRoomsScreens > MIN_SCREENS;
    }
}
