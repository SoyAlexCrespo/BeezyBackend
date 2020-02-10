using System;
using System.Collections.Generic;
using System.Text;

namespace BeezyBackend.Domain.Services
{
    public interface IWeekDates
    {
        public IEnumerable<DateTime> GetWeekDates(int weeksFromNow);
    }
}
