using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.DTOs
{
    public class PatchActorDTO
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
