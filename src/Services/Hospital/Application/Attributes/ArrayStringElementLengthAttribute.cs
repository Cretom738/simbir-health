using System.ComponentModel.DataAnnotations;

namespace Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ArrayStringElementLengthAttribute : ValidationAttribute
    {
        private readonly int _maxLength;

        public ArrayStringElementLengthAttribute(int maxLength)
        {
            _maxLength = maxLength;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            IList<string>? strings = (IList<string>?)value;

            if (strings == null || strings.Count == 0)
            {
                return ValidationResult.Success;
            }

            RequiredAttribute requiredAttribute = new RequiredAttribute
            {
                AllowEmptyStrings = false,
            };

            MaxLengthAttribute maxLengthAttribute = new MaxLengthAttribute(_maxLength);

            foreach (string str in strings)
            {
                if (!requiredAttribute.IsValid(str))
                {
                    return new ValidationResult(requiredAttribute.ErrorMessage);
                }
                if (!maxLengthAttribute.IsValid(str))
                {
                    return new ValidationResult(maxLengthAttribute.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
