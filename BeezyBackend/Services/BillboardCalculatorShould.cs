using BeezyBackend.Domain.Models;
using BeezyBackend.Domain.Services.QueryModels;
using BeezyBackend.Domain.Repositories;
using BeezyBackend.Domain.Services;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace BeezyBackend.Domain.UnitTest.Services
{

    public class BillboardCalculatorShould
    {
        private IMoviesRepository _repository;
        private IUtc _utc;
        private IWeekDates _weekDates;

        public BillboardCalculatorShould()
        {
            _repository = Substitute.For<IMoviesRepository>();
            _utc = Substitute.For<IUtc>();
            _utc.Now().Returns(new DateTime(2020, 02, 09));
            _weekDates = new WeekDates(_utc);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(-1, 10, 5)]
        [InlineData(1, 0, 0)]
        [InlineData(2, -3, 5)]
        [InlineData(2, 3, -5)]
        [InlineData(60, 3, 8)]
        [InlineData(12, 50, 8)]
        [InlineData(12, 5, 55)]
        public void Fail_When_Filters_Values_Are_OutOfRange(int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens)
        {
            //Arrange           

            //Act
            Action action = () => new BillboardCalculator(_repository, _weekDates, weeksFromNow, bigRoomsScreens, smallRoomsScreens, false);

            //Assert
            action.Should().Throw<ArgumentException>();

        }

        [Theory]
        [InlineData(10, 10, 10)]
        public async void Get_Empty_Board_When_There_Are_Not_Any_Movies(int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens)
        {
            //Arrange
            var billBoardCalculator = new BillboardCalculator(_repository, _weekDates, weeksFromNow, bigRoomsScreens, smallRoomsScreens, false);

            //Act
            var billBoard = await billBoardCalculator.GetBillboard();

            //Assert
            billBoard.GetWeekBoard().Should().BeEmpty();
        }

        [Theory]
        [InlineData(1, 1, 0)]
        [InlineData(1, 5, 0)]
        [InlineData(1, 7, 0)]
        public async void Get_Board_With_Many_BigRoom_Screen_Movie(int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens)
        {
            //Arrange
            var billBoardCalculator = new BillboardCalculator(_repository, _weekDates, weeksFromNow, bigRoomsScreens, smallRoomsScreens, false);
            var expectedMovies = BuildMovieInfosList();
            _repository.GetMoviesInfo().Returns(expectedMovies);

            //Act
            var billBoard = await billBoardCalculator.GetBillboard();

            //Assert
            billBoard.GetWeekBoard().Should().HaveCount(weeksFromNow);
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.Count == bigRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.First().Movie.Title == expectedMovies.First().Title && w.First().SmallScreenBoard.Count == smallRoomsScreens);
        }

        [Theory]
        [InlineData(1, 0, 1)]
        [InlineData(1, 0, 5)]
        public async void Get_Board_With_Many_SmallRoom_Screen_Movie(int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens)
        {
            //Arrange
            var billBoardCalculator = new BillboardCalculator(_repository, _weekDates, weeksFromNow, bigRoomsScreens, smallRoomsScreens, false);
            var expectedMovies = BuildMovieInfosList();
            _repository.GetMoviesInfo().Returns(expectedMovies);

            //Act
            var billBoard = await billBoardCalculator.GetBillboard();

            //Assert
            billBoard.GetWeekBoard().Should().HaveCount(weeksFromNow);
            billBoard.GetWeekBoard().Should().Match(w => w.First().SmallScreenBoard.Count == smallRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.First().SmallScreenBoard.First().Movie.Title == expectedMovies.Skip(5).First().Title && w.First().BigScreenBoard.Count == bigRoomsScreens);
        }

        [Theory]
        [InlineData(1, 3, 4)]
        public async void Get_Board_With_Many_Rooms_Screen_Movie(int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens)
        {
            //Arrange
            var billBoardCalculator = new BillboardCalculator(_repository, _weekDates, weeksFromNow, bigRoomsScreens, smallRoomsScreens, false);
            var expectedMovies = BuildMovieInfosList();
            _repository.GetMoviesInfo().Returns(expectedMovies);

            //Act
            var billBoard = await billBoardCalculator.GetBillboard();

            //Assert
            billBoard.GetWeekBoard().Should().HaveCount(weeksFromNow);
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.Count == bigRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.First().Movie.Title == expectedMovies.First().Title);

            billBoard.GetWeekBoard().Should().Match(w => w.First().SmallScreenBoard.Count == smallRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.First().SmallScreenBoard.First().Movie.Title == expectedMovies.Skip(5).First().Title && w.First().BigScreenBoard.Count == bigRoomsScreens);
        }

        [Theory]
        [InlineData(4, 1, 1)]
        public async void Get_Board_With_Many_Weeks(int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens)
        {
            //Arrange
            var billBoardCalculator = new BillboardCalculator(_repository, _weekDates, weeksFromNow, bigRoomsScreens, smallRoomsScreens, false);
            var expectedMovies = BuildMovieInfosList();
            _repository.GetMoviesInfo().Returns(expectedMovies);

            //Act
            var billBoard = await billBoardCalculator.GetBillboard();

            //Assert
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.Count == bigRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.First().Movie.Title == expectedMovies.First().Title);
            billBoard.GetWeekBoard().Should().Match(w => w.Skip(1).First().BigScreenBoard.First().Movie.Title == expectedMovies.Skip(2).First().Title);

            billBoard.GetWeekBoard().Should().HaveCount(weeksFromNow);
            billBoard.GetWeekBoard().Should().Match(w => w.First().SmallScreenBoard.Count == smallRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.First().SmallScreenBoard.First().Movie.Title == expectedMovies.Skip(5).First().Title && w.First().BigScreenBoard.Count == bigRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.Skip(1).First().SmallScreenBoard.First().Movie.Title == expectedMovies.Skip(4).First().Title && w.First().BigScreenBoard.Count == bigRoomsScreens);
        }

        [Theory]
        [InlineData(4, 1, 1)]
        public async void Not_Build_Weeks_With_Not_Released_Movies(int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens)
        {
            //Arrange
            _utc.Now().Returns(new DateTime(2019, 08, 01));
            var billBoardCalculator = new BillboardCalculator(_repository, _weekDates, weeksFromNow, bigRoomsScreens, smallRoomsScreens, false);
            var expectedMovies = BuildMovieInfosList();
            _repository.GetMoviesInfo().Returns(expectedMovies);


            //Act
            var billBoard = await billBoardCalculator.GetBillboard();

            //Assert
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.Count == bigRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.First().Movie.Title == expectedMovies.First().Title);
            billBoard.GetWeekBoard().Should().Match(w => w.Skip(1).First().BigScreenBoard.First().Movie.Title == expectedMovies.Skip(2).First().Title);
            billBoard.GetWeekBoard().Should().Match(w => w.Skip(2).First().BigScreenBoard.First().Movie.Title == expectedMovies.Skip(1).First().Title);

            billBoard.GetWeekBoard().Should().HaveCount(weeksFromNow);
            billBoard.GetWeekBoard().Should().Match(w => w.First().SmallScreenBoard.Count == smallRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.First().SmallScreenBoard.First().Movie.Title == expectedMovies.Skip(6).First().Title && w.First().BigScreenBoard.Count == bigRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.Skip(1).First().SmallScreenBoard.First().Movie.Title == expectedMovies.Skip(4).First().Title && w.First().BigScreenBoard.Count == bigRoomsScreens);
        }


        [Theory]
        [InlineData(3, 2, 2)]
        public async void Get_Board_Without_Total_Seat_Sold_And_Some_Gaps_When_There_Are_Not_Movies_Released_That_Week(int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens)
        {
            //Arrange
            _utc.Now().Returns(new DateTime(2019, 08, 01));
            var billBoardCalculator = new BillboardCalculator(_repository, _weekDates, weeksFromNow, bigRoomsScreens, smallRoomsScreens, false);
            var expectedMovies = BuildMovieInfosList(false);
            _repository.GetMoviesInfo().Returns(expectedMovies);


            //Act
            var billBoard = await billBoardCalculator.GetBillboard();

            //Assert

            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.Count == bigRoomsScreens);
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.First().Movie.Title == expectedMovies.First().Title);
            billBoard.GetWeekBoard().Should().Match(w => w.First().BigScreenBoard.Skip(1).First().Movie.Title == expectedMovies.Skip(2).First().Title);
            billBoard.GetWeekBoard().Should().Match(w => w.Skip(1).First().BigScreenBoard.First().Movie == null); //Gap
            billBoard.GetWeekBoard().Should().Match(w => w.Skip(2).First().BigScreenBoard.First().Movie.Title == expectedMovies.Skip(1).First().Title);
        }


        private List<MovieInfo> BuildMovieInfosList(bool hasTotalSeat = true)
        {


            return new List<MovieInfo> {
                new MovieInfo("Spider-Man", "A man bited by radiactive spider", "action", "EN", new DateTime(2019, 1, 1), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 100 : 0),
                new MovieInfo("Ant-Man", "A man bited by radiactive ant", "action", "EN", new DateTime(2019, 8, 19), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 80 : 0),
                new MovieInfo("Super-Man", "A man bited by radiactive bird", "action", "EN", new DateTime(2019, 7, 1), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 90 : 0),
                new MovieInfo("Boogie-Man", "A man possessed by boogie-woogie spirit", "terror", "EN", new DateTime(2020, 6, 6), "", new List<string>(), MovieInfo.RoomSizeType.Big, hasTotalSeat ? 70 : 0),
                new MovieInfo("Mamma Mia!", "A woman possessed by Abba spirit", "musical", "EN", new DateTime(2019, 8, 6), "", new List<string>(), MovieInfo.RoomSizeType.Small, hasTotalSeat ? 20 : 0),
                new MovieInfo("Oceans Eleven", "A lot of men possessed by El Dioni spirit", "comedy", "EN", new DateTime(2020, 2, 1), "", new List<string>(), MovieInfo.RoomSizeType.Small, hasTotalSeat ? 85 : 0),
                new MovieInfo("The Matrix", "A man bited by a possessed IA", "CIFI", "EN", new DateTime(2018, 6, 6), "", new List<string>(), MovieInfo.RoomSizeType.Small, hasTotalSeat ? 15 : 0),
            };
        }
    }
}
