using System;
using System.Collections.Generic;

namespace BeezyBackend.Domain.Models
{
    public class Session
    {
        public int Id { get; private set; }
        public int RoomId { get; private set; }
        public int MovieId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int? SeatsSold { get; private set; }

        public virtual Movie Movie { get; private set; }
        public virtual Room Room { get; private set; }
    }
}
