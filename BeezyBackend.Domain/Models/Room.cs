using System;
using System.Collections.Generic;

namespace BeezyBackend.Domain.Models
{
    public class Room
    {
        public Room()
        {
            Session = new HashSet<Session>();
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Size { get; private set; }
        public int Seats { get; private set; }
        public int CinemaId { get; private set; }

        public virtual Cinema Cinema { get; private set; }
        public virtual ICollection<Session> Session { get; private set; }
    }
}
