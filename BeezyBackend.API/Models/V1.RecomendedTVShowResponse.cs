using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeezyBackend.API.Models
{
    public static partial class V1
    {
        public sealed class RecomendedTVShowResponse : RecomendedResponse
        {
            public int NumberOfSeasons { get; set; }
            public int NumberOfEpisodes { get; set; }
            public bool IsConcluded { get; set; }

        }
    }
}
