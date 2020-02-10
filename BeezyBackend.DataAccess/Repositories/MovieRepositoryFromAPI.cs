using BeezyBackend.DataAccess.DBContext;
using BeezyBackend.Domain.Models;
using BeezyBackend.Domain.Repositories;
using BeezyBackend.Domain.Services.QueryModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Util;
using System.Globalization;

namespace BeezyBackend.DataAccess.Repositories
{
    internal class MovieRepositoryFromAPI
    {
        private const string DISCOVER_MOVIE_URL = "https://api.themoviedb.org/3/discover/movie";
        private const string GENRE_LIST_URL = "https://api.themoviedb.org/3/genre/movie/list";
        private const string API_KEY = "5d43fae7a264940bf42f82f6d63aa049";
        private const string SORT_BY = "popularity.desc";

        internal async Task<List<MovieInfo>> GetMoviesInfo()
        {
            var bigMovies = await DISCOVER_MOVIE_URL
                .SetQueryParams(new
                {
                    api_key = API_KEY,
                    sort_by = SORT_BY,
                    include_adult = false,
                    include_video = false,
                    page = 1 //Big Rooms Movies
                })
                .GetJsonAsync();

            var smallMovies = await DISCOVER_MOVIE_URL
                .SetQueryParams(new
                {
                    api_key = API_KEY,
                    sort_by = SORT_BY,
                    include_adult = false,
                    include_video = false,
                    page = 2 //Small Rooms Movies
                })
                .GetJsonAsync();

            var genres = await GENRE_LIST_URL
                .SetQueryParams(new
                {
                    api_key = API_KEY,
                })
                .GetJsonAsync();

            Dictionary<long, string> genresDictionary = SetGenres(genres);

            return GetMoviesInfoList(bigMovies, smallMovies, genresDictionary);
        }

        private List<MovieInfo> GetMoviesInfoList(dynamic bigMovies, dynamic smallMovies, Dictionary<long, string> genresDictionary)
        {
            var movies = new List<MovieInfo>();

            foreach (var movie in bigMovies.results)
            {
                movies.Add(ConvertToMovieInfo(movie, genresDictionary, MovieInfo.RoomSizeType.Big));
            }

            foreach (var movie in smallMovies.results)
            {
                movies.Add(ConvertToMovieInfo(movie, genresDictionary, MovieInfo.RoomSizeType.Small));
            }
            return movies;
        }
        private MovieInfo ConvertToMovieInfo(dynamic movie, Dictionary<long, string> genres, MovieInfo.RoomSizeType roomSize)
        {
            string title = movie.title;
            string overview = movie.overview;
            string movieGenre = movie.genre_ids.Count > 0 ? genres[movie.genre_ids[0]] : null;
            string original_language = movie.original_language;
            DateTime release_date = DateTime.ParseExact(movie.release_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return new MovieInfo(title, overview, movieGenre, original_language, release_date, string.Empty, new List<string>(), roomSize, 0);
        }

        private Dictionary<long, string> SetGenres(dynamic genres)
        {
            Dictionary<long, string> genresDictionary = new Dictionary<long, string>();

            foreach (var genre in genres.genres)
            {
                genresDictionary.Add(genre.id, genre.name);
            }

            return genresDictionary;
        }
    }
}
