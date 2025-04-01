using System.ComponentModel.DataAnnotations;
using SeaEco.Abstractions.Resources;

namespace SeaEco.Abstractions.Models.Authentication;

public sealed class RegisterUserDto
{
    [Required(ErrorMessageResourceName = "ErrorMessageFirstNameRequired", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [MinLength(3, ErrorMessageResourceName = "ErrorMessageFirstNameMinLength", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessageResourceName = "ErrorMessageLastNameRequired", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [MinLength(3, ErrorMessageResourceName = "ErrorMessageLastNameMinLength", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessageResourceName = "ErrorMessageMailRequired", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [EmailAddress(ErrorMessageResourceName = "ErrorMessageMailError", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessageResourceName = "ErrorMessagePasswordRequired", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [MinLength(8, ErrorMessageResourceName = "ErrorMessagePasswordMinLength", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).+$", ErrorMessageResourceName = "ErrorMessagePasswordContain", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessageResourceName = "ErrorMessageRepPasswordRequired", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [Compare("Password", ErrorMessageResourceName = "ErrorMessageRepPasswordCompare", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string ConfirmPassword { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
}