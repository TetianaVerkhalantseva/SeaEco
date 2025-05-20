using System.Text.RegularExpressions;
using FluentValidation;
using SeaEco.Abstractions.Models.User;

namespace SeaEco.Services.Validators;


public sealed class EditUserDtoValidator : AbstractValidator<EditUserDto>
{
    private const string EmailValidationPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    
    private const string InvalidEmailFormatError = "Ugyldig e-postadresse";
    private const string EmailIsEmptyError = "E-postadresse er påkrevd";
    
    private const string FirstNameIsRequiredError = "Fornavn er påkrevd";
    private const string FirstNameShortError = "Fornavnet er for kort";
    private const string FirstNameLongError = "Fornavnet er for langt";

    private const string LastNameIsRequiredError = "Etternavn er påkrevd";
    private const string LastNameShortError = "Etternavnet er for kort";
    private const string LastNameLongError = "Etternavnet er for langt";
    
    public EditUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(EmailIsEmptyError)
            .NotNull().WithMessage(EmailIsEmptyError)
            .Must(email => Regex.IsMatch(email, EmailValidationPattern)).WithMessage(InvalidEmailFormatError);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(FirstNameIsRequiredError)
            .NotNull().WithMessage(FirstNameIsRequiredError)
            .MinimumLength(3).WithMessage(FirstNameShortError)
            .MaximumLength(63).WithMessage(FirstNameLongError);
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(LastNameIsRequiredError)
            .NotNull().WithMessage(LastNameIsRequiredError)
            .MinimumLength(3).WithMessage(LastNameShortError)
            .MaximumLength(63).WithMessage(LastNameLongError);
    }
}
