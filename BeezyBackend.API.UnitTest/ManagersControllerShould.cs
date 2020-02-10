using BeezyBackend.API.Controllers;
using BeezyBackend.API.Models;
using BeezyBackend.Domain.Repositories;
using BeezyBackend.Domain.Services;
using BeezyBackend.Domain.Services.QueryModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BeezyBackend.API.UnitTest
{
    public class ManagersControllerShould
    {

        private IMoviesRepository _repository;
        private IUtc _utc;
        private IWeekDates _weekDates;
        private ILogger<ManagersController> _logger;

        public ManagersControllerShould()
        {
            _repository = Substitute.For<IMoviesRepository>();
            _utc = Substitute.For<IUtc>();
            _utc.Now().Returns(new DateTime(2020, 02, 09));
            _weekDates = new WeekDates(_utc);
            _logger = Substitute.For<ILogger<ManagersController>>();
        }

        [Fact]
        public async void Get_Movies_From_City_Seats_Sold()
        {
            //Arrange
            var expectedMoviesCity = BuildMovieInfosListFromCity();
            _repository.GetMoviesInfo(true).Returns(expectedMoviesCity);

            var expectedMoviesFilters = BuildMovieInfosListFromFilters();
            _repository.GetMoviesInfo(false).Returns(expectedMoviesFilters);

            var controller = new ManagersController(_repository, _weekDates, _logger);

            //Act
            var result = await controller.GetIntelligentBillboard(1, 1, 1, true);

            //Assert
            var typedResultGet = (List<V1.IntelligentBillboardResponse>)Assert.IsType<OkObjectResult>(result).Value;
            typedResultGet.Should().Match<List<V1.IntelligentBillboardResponse>>(m => m.First().BigScreensMovies != null);
            typedResultGet.Should().Match<List<V1.IntelligentBillboardResponse>>(m => m.First().BigScreensMovies.First().Movie.Title == "Spider-Man");

        }

        [Fact]
        public async void Get_Movies_From_Filters()
        {
            //Arrange
            var expectedMoviesCity = BuildMovieInfosListFromCity();
            _repository.GetMoviesInfo(true).Returns(expectedMoviesCity);

            var expectedMoviesFilters = BuildMovieInfosListFromFilters();
            _repository.GetMoviesInfo(false).Returns(expectedMoviesFilters);

            var controller = new ManagersController(_repository, _weekDates, _logger);

            //Act
            var result = await controller.GetIntelligentBillboard(1, 1, 1, false);

            //Assert
            var typedResultGet = (List<V1.IntelligentBillboardResponse>)Assert.IsType<OkObjectResult>(result).Value;
            typedResultGet.Should().Match<List<V1.IntelligentBillboardResponse>>(m => m.First().BigScreensMovies != null);
            typedResultGet.Should().Match<List<V1.IntelligentBillboardResponse>>(m => m.First().BigScreensMovies.First().Movie.Title == "Boogie-Man");

        }


        private List<MovieInfo> BuildMovieInfosListFromCity(bool hasTotalSeat = true)
        {
            return new List<MovieInfo> {
                new MovieInfo("Spider-Man", "A man bited by radiactive spider", "action", "EN", new DateTime(2019, 1, 1), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 100 : 0),
                //new MovieInfo("Ant-Man", "A man bited by radiactive ant", "action", "EN", new DateTime(2019, 8, 19), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 80 : 0),
                //new MovieInfo("Super-Man", "A man bited by radiactive bird", "action", "EN", new DateTime(2019, 7, 1), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 90 : 0),
                //new MovieInfo("Boogie-Man", "A man possessed by boogie-woogie spirit", "terror", "EN", new DateTime(2020, 6, 6), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 70 : 0),
                //new MovieInfo("Mamma Mia!", "A woman possessed by Abba spirit", "musical", "EN", new DateTime(2019, 8, 6), "", new List<string>(), MovieInfo.RoomSizeType.Small, hasTotalSeat ? 20 : 0),
                //new MovieInfo("Oceans Eleven", "A lot of men possessed by El Dioni spirit", "comedy", "EN", new DateTime(2020, 2, 1), "", new List<string>(), MovieInfo.RoomSizeType.Small, hasTotalSeat ? 85 : 0),
                //new MovieInfo("The Matrix", "A man bited by a possessed IA", "CIFI", "EN", new DateTime(2018, 6, 6), "", new List<string>(), MovieInfo.RoomSizeType.Small, hasTotalSeat ? 15 : 0),
            };
        }

        private List<MovieInfo> BuildMovieInfosListFromFilters(bool hasTotalSeat = true)
        {
            return new List<MovieInfo> {
                //new MovieInfo("Spider-Man", "A man bited by radiactive spider", "action", "EN", new DateTime(2019, 1, 1), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 100 : 0),
                //new MovieInfo("Ant-Man", "A man bited by radiactive ant", "action", "EN", new DateTime(2019, 8, 19), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 80 : 0),
                //new MovieInfo("Super-Man", "A man bited by radiactive bird", "action", "EN", new DateTime(2019, 7, 1), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 90 : 0),
                new MovieInfo("Boogie-Man", "A man possessed by boogie-woogie spirit", "terror", "EN", new DateTime(2010, 6, 6), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 70 : 0),
                //new MovieInfo("Mamma Mia!", "A woman possessed by Abba spirit", "musical", "EN", new DateTime(2019, 8, 6), "", new List<string>(), MovieInfo.RoomSizeType.Small, hasTotalSeat ? 20 : 0),
                //new MovieInfo("Oceans Eleven", "A lot of men possessed by El Dioni spirit", "comedy", "EN", new DateTime(2020, 2, 1), "", new List<string>(), MovieInfo.RoomSizeType.Small, hasTotalSeat ? 85 : 0),
                //new MovieInfo("The Matrix", "A man bited by a possessed IA", "CIFI", "EN", new DateTime(2018, 6, 6), "", new List<string>(), MovieInfo.RoomSizeType.Small, hasTotalSeat ? 15 : 0),
            };
        }
    }
}
