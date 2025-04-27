using System.ComponentModel.DataAnnotations;
using SeaEco.Abstractions.Resources;

namespace SeaEco.Abstractions.Models.Authentication;

public class ResetPasswordConfirmDto
{
    [Required(ErrorMessageResourceName = "ErrorMessagePasswordRequired", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [MinLength(8, ErrorMessageResourceName = "ErrorMessagePasswordMinLength", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).+$", ErrorMessageResourceName = "ErrorMessagePasswordContain", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessageResourceName = "ErrorMessageRepPasswordRequired", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    [Compare("Password", ErrorMessageResourceName = "ErrorMessageRepPasswordCompare", ErrorMessageResourceType = typeof(ResourcesAbstractions))]
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}