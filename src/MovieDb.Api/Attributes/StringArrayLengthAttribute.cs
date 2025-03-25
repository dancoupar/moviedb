using System.ComponentModel.DataAnnotations;

namespace MovieDb.Api.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class StringArrayLengthAttribute : ValidationAttribute
	{
		private readonly int _maxLength;

		public StringArrayLengthAttribute(int maxLength)
		{
			_maxLength = maxLength;
			this.ErrorMessage = $"Each string must be a maximum of {maxLength} characters.";
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value is string[] stringArray)
			{
				if (stringArray.Any(s => s != null && s.Length > _maxLength))
				{
					return new ValidationResult(this.ErrorMessage);
				}
			}

			return ValidationResult.Success;
		}
	}
}
