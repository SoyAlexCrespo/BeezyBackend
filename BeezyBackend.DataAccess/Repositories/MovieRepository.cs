using BeezyBackend.DataAccess.DBContext;
using BeezyBackend.Domain.Models;
using BeezyBackend.Domain.Repositories;
using BeezyBackend.Domain.Services.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeezyBackend.DataAccess.Repositories
{
    public class MovieRepository : IMoviesRepository
    {
        private readonly BeezyCinemaContext _context;

        public MovieRepository(BeezyCinemaContext context)
        {
            _context = context;
        }

        public async Task<List<MovieInfo>> GetMoviesInfo(bool basedOnCityMovies)
        {
            if (basedOnCityMovies) return await GetMoviesInfoFromDB();
            return await new MovieRepositoryFromAPI().GetMoviesInfo();
        }

        private async Task<List<MovieInfo>> GetMoviesInfoFromDB()
        {
            var movies = await _context.Movie.Include(m => m.Session).ThenInclude(s => s.Room).ToListAsync();

            var moviesInfo = movies.SelectMany(m => m.Session.Select(s => new { movie = m, seats = s.SeatsSold, size = s.Room?.Size }));

            var moviesgenres = await _context.MovieGenre.ToListAsync();
            var genres = await _context.Genre.ToListAsync();

            var groupMovies = moviesInfo.GroupBy(m => new { m.movie, m.size }, m => m.seats, (key, g) => new  { movieSize = key, suma = g.Sum() })
                                        .Select(n => new MovieInfo
                                        (
                                            title: n.movieSize.movie.OriginalTitle, 
                                            overview: string.Empty, 
                                            genre: GetGenre(n.movieSize.movie.Id, moviesgenres, genres),
                                            language: n.movieSize.movie.OriginalLanguage, 
                                            releaseDate: n.movieSize.movie.ReleaseDate, 
                                            webSite: string.Empty, 
                                            keywords: new List<string>(),                                            
                                            roomSize: GetRoomSizeType(n.movieSize.size), 
                                            totalSeatsSold: n.suma ?? 0));

            return groupMovies.ToList();
        }

        private string GetGenre(int id, List<MovieGenre> moviesgenres, List<Genre> genres)
        {
            var genreId = moviesgenres.FirstOrDefault(mg => mg.MovieId == id).GenreId;
            return genres.FirstOrDefault(g => g.Id == genreId).Name;
        }

        private MovieInfo.RoomSizeType GetRoomSizeType(string size)
        {
            if (size?.ToLower() == "small") return MovieInfo.RoomSizeType.Small;
            return MovieInfo.RoomSizeType.Big;
        }
    }
}
