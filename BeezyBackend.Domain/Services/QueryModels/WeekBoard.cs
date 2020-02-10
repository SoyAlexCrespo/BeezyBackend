using System;
using System.Collections.Generic;
using System.Linq;

namespace BeezyBackend.Domain.Services.QueryModels
{
    public class WeekBoard
    {

        public WeekBoard(DateTime weekDate)
        {
            WeekStart = weekDate;
            BigScreenBoard = new List<ScreenBoard>();
            SmallScreenBoard = new List<ScreenBoard>();
        }

        public DateTime WeekStart { get; private set; }

        public List<ScreenBoard> BigScreenBoard { get; private set; }

        public List<ScreenBoard> SmallScreenBoard { get; private set; }

        public void BuildBigScreenBoard(List<MovieInfo> bigMoviesList, int bigRoomsScreens)
        {
            BigScreenBoard = GetScreenBoardFromList(bigMoviesList, bigRoomsScreens).ToList();
        }

        public void BuildSmallScreenBoard(List<MovieInfo> smallMoviesList, int smallRoomsScreens)
        {
            SmallScreenBoard = GetScreenBoardFromList(smallMoviesList, smallRoomsScreens).ToList();
        }

        private IEnumerable<ScreenBoard> GetScreenBoardFromList(List<MovieInfo> moviesList, int roomsScreens)
        {

            for (int screenIndex = 1; screenIndex <= roomsScreens; screenIndex++)
            {
                var movie = GetNextMovieFromList(moviesList);

                if (movie != null) moviesList.Remove(movie);

                yield return new ScreenBoard(screenIndex, movie);
            }
        }

        private MovieInfo GetNextMovieFromList(List<MovieInfo> moviesList)
        {
            return moviesList.FirstOrDefault(m => m.ReleaseDate <= WeekStart);
        }
    }
}