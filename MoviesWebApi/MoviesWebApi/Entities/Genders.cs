using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.Entities
{
    public class Genders
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
