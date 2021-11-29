using MoviesWebApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.DTOs
{
    public class CreationActorDTO:PatchActorDTO
    {
     
        [WeightFileValidation(maxWeight:4)]
        [ValidationTypeFile(typeGroupFile: TypeGroupFile.Imagen)]
        public IFormFile Picture { get; set; }

    }
}
