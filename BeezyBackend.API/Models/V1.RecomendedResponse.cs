using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeezyBackend.API.Models
{
    public static partial class V1
    {
        public abstract class RecomendedResponse
        {
            public string Title { get; set; }
            public string Overview { get; set; }
            public string Genre { get; set; }
            public string Language { get; set; }
            public DateTime ReleaseDate { get; set; }
            public string WebSite { get; set; }
            public List<string> Keywords { get; set; }
        }
    }
}
