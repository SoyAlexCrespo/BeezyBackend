using System;
using System.Collections.Generic;

namespace BeezyBackend.Domain.Models
{
    public class City
    {
        public City()
        {
            Cinema = new HashSet<Cinema>();
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Population { get; private set; }

        public virtual ICollection<Cinema> Cinema { get; private set; }
    }
}
