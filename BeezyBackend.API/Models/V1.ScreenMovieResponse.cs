using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeezyBackend.API.Models
{
    public static partial class V1
    {
        public class ScreenMovieResponse
        {
            public int Screen { get; set; }
            public RecomendedMovieResponse Movie{ get; set; }
        }
    }
}
