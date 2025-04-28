using System.ComponentModel.DataAnnotations;
using SeaEco.Abstractions.Resources;

namespace SeaEco.Abstractions.Models.User;

public sealed class EditUserDto
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
    public bool IsAdmin { get; set; }
    public bool IsActive { get; set; }
}