using System;
using System.Collections.Generic;

namespace BeezyBackend.Domain.Models
{
    public class Cinema
    {
        public Cinema()
        {
            Room = new HashSet<Room>();
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTime OpenSince { get; private set; }
        public int CityId { get; private set; }

        public virtual City City { get; private set; }
        public virtual ICollection<Room> Room { get; private set; }
    }
}
