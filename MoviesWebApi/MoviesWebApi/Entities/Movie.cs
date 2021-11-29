using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool OnTheather { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string  Poster { get; set; }
    }
}
