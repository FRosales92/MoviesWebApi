using MoviesWebApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.DTOs
{
    public class CreationActorDTO
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        [WeightFileValidation(maxWeight:4)]
        [ValidationTypeFile(typeGroupFile: TypeGroupFile.Imagen)]
        public IFormFile Picture { get; set; }

    }
}
