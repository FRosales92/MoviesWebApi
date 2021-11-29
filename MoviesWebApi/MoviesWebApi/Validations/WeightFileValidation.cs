using System.ComponentModel.DataAnnotations;

namespace MoviesWebApi.Validations
{
    public class WeightFileValidation: ValidationAttribute
    {
        private readonly int maxWeight;

        public WeightFileValidation(int maxWeight)
        {
            this.maxWeight = maxWeight;
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

            if (formFile.Length > maxWeight * 1024 * 1024)
            {
                return new ValidationResult($"The weight can be more than {maxWeight} mb");
            }

            return ValidationResult.Success;

        }

    }
}
