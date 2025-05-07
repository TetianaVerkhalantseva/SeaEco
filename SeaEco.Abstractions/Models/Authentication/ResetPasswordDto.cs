using System.ComponentModel.DataAnnotations;
using SeaEco.Abstractions.Resources;

namespace SeaEco.Abstractions.Models.Authentication;

public sealed class ResetPasswordDto
{
    [Required(ErrorMessageResourceName = "ErrorMessageMailRequired", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [EmailAddress(ErrorMessageResourceName = "ErrorMessageMailError", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string Email { get; set; } = string.Empty;
}