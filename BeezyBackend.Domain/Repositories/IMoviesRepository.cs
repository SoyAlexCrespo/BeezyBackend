using BeezyBackend.Domain.Models;
using BeezyBackend.Domain.Services.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeezyBackend.Domain.Repositories
{
    public interface IMoviesRepository
    {
        public Task<List<MovieInfo>> GetMoviesInfo(bool basedOnCityMovies = false);
    }
}
