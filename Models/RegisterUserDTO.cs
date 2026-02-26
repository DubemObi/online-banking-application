using System.ComponentModel.DataAnnotations;


public class RegisterUserDTO
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
    [Required]
    [StringLength(200, MinimumLength = 10)]
    public string Address { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [MinimumAge(18)]
    public DateTime DateOfBirth { get; set; }

}


public class MinimumAgeAttribute : ValidationAttribute
{
    private readonly int _minimumAge;

    public MinimumAgeAttribute(int minimumAge)
    {
        _minimumAge = minimumAge;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime date)
        {
            var age = DateTime.Today.Year - date.Year;
            if (date > DateTime.Today.AddYears(-age)) age--;

            if (age >= _minimumAge)
                return ValidationResult.Success;
        }

        return new ValidationResult($"You must be at least {_minimumAge} years old.");
    }
}