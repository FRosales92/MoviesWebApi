using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.DTOs
{
    public class PatchMovieDTO
    {
       
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool OnTheather { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
