using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool OnTheather { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
    }
}
