using System;
using System.Collections.Generic;
using System.Text;

namespace BeezyBackend.Domain.Services.QueryModels
{
    public class WeekDates : IWeekDates
    {
        private readonly IUtc _utc;
        private const DayOfWeek WEEK_START = DayOfWeek.Monday;

        public WeekDates(IUtc utc)
        {
            _utc = utc;
        }
        public IEnumerable<DateTime> GetWeekDates(int weeksFromNow)
        {
            for (int weekIndex = 0; weekIndex < weeksFromNow; weekIndex++)
            {
                yield return StartOfWeek(_utc.Now().AddDays(7 * (weekIndex + 1)), WEEK_START);
            }
        }

        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
