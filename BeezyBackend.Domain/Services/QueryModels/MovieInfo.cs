using System;
using System.Collections.Generic;
using System.Text;

namespace BeezyBackend.Domain.Services.QueryModels
{
    public class MovieInfo : MovieFile
    {
        public enum RoomSizeType
        {
            Big,
            Small,
        }
        
        public RoomSizeType RoomSize { get; private set; }
        public int TotalSeatsSold { get; private set; }

        public MovieInfo(string title, string overview, string genre, string language, DateTime releaseDate, string webSite, List<string> keywords, RoomSizeType roomSize, int totalSeatsSold) : 
            base (title, overview, genre, language, releaseDate, webSite, keywords)
        {            
            RoomSize = roomSize;
            TotalSeatsSold = totalSeatsSold;
        }
        public MovieInfo(MovieFile movie, RoomSizeType roomSize, int totalSeatsSold) :
            base(movie)
        {
            RoomSize = roomSize;
            TotalSeatsSold = totalSeatsSold;
        }
    }

}
