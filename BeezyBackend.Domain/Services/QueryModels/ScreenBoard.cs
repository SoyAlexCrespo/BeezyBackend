namespace BeezyBackend.Domain.Services.QueryModels
{
    public class ScreenBoard
    {
        public int Screen { get; private set; }

        public MovieFile Movie { get; private set; }

        public ScreenBoard(int screen, MovieFile movie)
        {
            Screen = screen;
            Movie = movie;
        }
    }
}