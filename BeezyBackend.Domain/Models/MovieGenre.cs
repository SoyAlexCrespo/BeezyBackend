using System;
using System.Collections.Generic;

namespace BeezyBackend.Domain.Models
{
    public class MovieGenre
    {
        public int MovieId { get; private set; }
        public int GenreId { get; private set; }
    }
}
