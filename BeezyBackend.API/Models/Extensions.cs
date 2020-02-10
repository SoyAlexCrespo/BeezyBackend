using BeezyBackend.Domain.Services.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeezyBackend.API.Models
{
    public static class Extensions
    {
        public static List<V1.IntelligentBillboardResponse> ToDto(this Billboard billboard)
        {
            return GetBillboardResponse(billboard).ToList();

        }

        private static IEnumerable<V1.IntelligentBillboardResponse> GetBillboardResponse(Billboard billboard)
        {
            foreach (var weekBoard in billboard.GetWeekBoard())
            {
                yield return new V1.IntelligentBillboardResponse()
                {
                    WeekStart = weekBoard.WeekStart,
                    BigScreensMovies = weekBoard.BigScreenBoard.ConvertAll(s => new V1.ScreenMovieResponse() { Movie = s.Movie?.ToDto(), Screen = s.Screen }),
                    SmallScreensMovies = weekBoard.SmallScreenBoard.ConvertAll(s => new V1.ScreenMovieResponse() { Movie = s.Movie?.ToDto(), Screen = s.Screen }),
                };
            }
        }

        public static V1.RecomendedMovieResponse ToDto(this MovieFile movie)
        {
            return new V1.RecomendedMovieResponse()
            {
                Title = movie.Title,
                Genre = movie.Genre,
                Language = movie.Language,
                Overview = movie.Overview,
                ReleaseDate = movie.ReleaseDate,
                Keywords = movie.Keywords,
                WebSite = movie.WebSite
            };

        }
    }
}
