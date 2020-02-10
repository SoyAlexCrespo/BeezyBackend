using System;
using System.Collections.Generic;

namespace BeezyBackend.Domain.Services.QueryModels
{
    public class MovieFile
    {
        public string Title { get; private set; }
        public string Overview { get; private set; }
        public string Genre { get; private set; }
        public string Language { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public string WebSite { get; private set; }
        public List<string> Keywords { get; private set; }

        public MovieFile(string title, string overview, string genre, string language, DateTime releaseDate, string webSite, List<string> keywords)
        {
            Title = title;
            Overview = overview;
            Genre = genre;
            Language = language;
            ReleaseDate = releaseDate;
            WebSite = webSite;
            Keywords = keywords;
        }

        public MovieFile(MovieFile movie)
        {
            Title = movie.Title;
            Overview = movie.Overview;
            Genre = movie.Genre;
            Language = movie.Language;
            ReleaseDate = movie.ReleaseDate;
            WebSite = movie.WebSite;
            Keywords = movie.Keywords;
        }
    }
}