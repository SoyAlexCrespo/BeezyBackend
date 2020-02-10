using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeezyBackend.API.Models
{
    public static partial class V1
    {
        public class BillboardResponse
        {
            public DateTime WeekStart { get; set; }
            public List<ScreenMovieResponse> ScreenMovie { get; set; }
        }
    }
}
