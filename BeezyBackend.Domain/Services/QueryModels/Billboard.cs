using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BeezyBackend.Domain.Services.QueryModels
{
    public class Billboard
    {
        private readonly List<WeekBoard> _weekBoard;
        private readonly List<MovieInfo> _moviesInfo;
        private readonly IWeekDates _weekDates;
        private readonly int _weeksFromNow;
        private readonly int _bigRoomsScreens;
        private readonly int _smallRoomsScreens;

        public Billboard(List<MovieInfo> moviesInfo, IWeekDates weekDates, int weeksFromNow, int bigRoomsScreens, int smallRoomsScreens)
        {
            _moviesInfo = moviesInfo;
            _weekDates = weekDates;
            _weeksFromNow = weeksFromNow;
            _bigRoomsScreens = bigRoomsScreens;
            _smallRoomsScreens = smallRoomsScreens;

            _weekBoard = GetWeekBoardFromMoviesInfo().ToList();
        }

        public ReadOnlyCollection<WeekBoard> GetWeekBoard() => _weekBoard.AsReadOnly();

        private IEnumerable<WeekBoard> GetWeekBoardFromMoviesInfo()
        {
            if (_moviesInfo == null || _moviesInfo.Count == 0) yield break;

            var bigMoviesStack = new List<MovieInfo>(_moviesInfo.Where(m => m.RoomSize == MovieInfo.RoomSizeType.Big).OrderByDescending(m => m.TotalSeatsSold));
            var smallMoviesStack = new List<MovieInfo>(_moviesInfo.Where(m => m.RoomSize == MovieInfo.RoomSizeType.Small).OrderByDescending(m => m.TotalSeatsSold));

            foreach (var weekDate in _weekDates.GetWeekDates(_weeksFromNow))
            {
                var weekBoard = new WeekBoard(weekDate);
                weekBoard.BuildBigScreenBoard(bigMoviesStack, _bigRoomsScreens);
                weekBoard.BuildSmallScreenBoard(smallMoviesStack, _smallRoomsScreens);
                yield return weekBoard;
            }
        }


    }
}