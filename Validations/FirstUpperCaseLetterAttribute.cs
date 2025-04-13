using System.ComponentModel.DataAnnotations;

namespace ManejadorPresupuestos.Validations
{
    public class FirstUpperCaseLetterAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if(value == null) return ValidationResult.Success;

            var firstLetter = value.ToString()[0].ToString();

            if (firstLetter != firstLetter.ToUpper()) return new ValidationResult("La primer letra debe ser mayúscula");

            return ValidationResult.Success;
            
        }
    }
}
