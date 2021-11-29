using MoviesWebApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.DTOs
{
    public class CreationMovieDTO: PatchMovieDTO
    {
       
        [WeightFileValidation(maxWeight: 4)]
        [ValidationTypeFile(typeGroupFile: TypeGroupFile.Imagen)]
        public IFormFile Poster { get; set; }
    }
}
