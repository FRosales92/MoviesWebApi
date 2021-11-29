using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.Validations
{
    public class ValidationTypeFile: ValidationAttribute
    {
        private readonly string[] validTypes;

        public ValidationTypeFile(string[] validTypes)
        {
            this.validTypes = validTypes;
        }
        public ValidationTypeFile(TypeGroupFile typeGroupFile)
        {
            if (typeGroupFile == TypeGroupFile.Imagen)
            {
                 validTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }
            if (!validTypes.Contains(formFile.ContentType))
            {
                return new ValidationResult($"The file type must be one of the following types: {string.Join(", ",validTypes)}");
            }
            return ValidationResult.Success;
        }
    }
}
