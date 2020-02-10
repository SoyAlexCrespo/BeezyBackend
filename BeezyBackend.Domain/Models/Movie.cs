using System;
using System.Collections.Generic;

namespace BeezyBackend.Domain.Models
{
    public class Movie
    {
        public Movie()
        {
            Session = new HashSet<Session>();
        }

        public Movie(string originalTitle, DateTime releaseDate, string originalLanguage, bool adult)
        {
            OriginalTitle = originalTitle;
            ReleaseDate = releaseDate;
            OriginalLanguage = originalLanguage;
            Adult = adult;

            Session = new HashSet<Session>();
        }

        public int Id { get; private set; }
        public string OriginalTitle { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public string OriginalLanguage { get; private set; }
        public bool Adult { get; private set; }
        public virtual ICollection<Session> Session { get; private set; }
    }
}
