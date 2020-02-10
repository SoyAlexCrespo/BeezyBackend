using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeezyBackend.API.Models
{
    public static partial class V1
    {
        public class IntelligentBillboardResponse
        {
            public DateTime WeekStart { get; set; }
            public List<ScreenMovieResponse> BigScreensMovies { get; set; }
            public List<ScreenMovieResponse> SmallScreensMovies { get; set; }
        }
    }
}
