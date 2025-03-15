using System.Text.RegularExpressions;
using FluentValidation;
using SeaEco.Abstractions.Models.Authentication;

namespace SeaEco.Services.Validators;

public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
{
    private const string EmailValidationPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    
    private const string InvalidEmailFormatError = "Ugyldig e-postadresse";
    private const string EmailIsEmptyError = "E-postadresse er påkrevd";
    
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(EmailIsEmptyError)
            .NotNull().WithMessage(EmailIsEmptyError)
            .Must(email => Regex.IsMatch(email, EmailValidationPattern)).WithMessage(InvalidEmailFormatError);
    }
}

public sealed class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    private const string EmailValidationPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    private const string PasswordValidationPattern = @"^(?=.*[A-Z])(?=.*[\W_]).{8,}$";
    
    private const string InvalidEmailFormatError = "Ugyldig e-postadresse";
    private const string EmailIsEmptyError = "E-postadresse er påkrevd";
    
    private const string InvalidPasswordError = "Passordet må være minst 8 tegn langt, inneholde minst én stor bokstav og ett spesialtegn";
    private const string PasswordIsEmptyError = "Passord er påkrevd";
    private const string PasswordsNotEqualError = "Passordene er ikke like";
    
    private const string FirstNameIsRequiredError = "Fornavn er påkrevd";
    private const string FirstNameShortError = "Fornavnet er for kort";
    private const string FirstNameLongError = "Fornavnet er for langt";

    private const string LastNameIsRequiredError = "Etternavn er påkrevd";
    private const string LastNameShortError = "Etternavnet er for kort";
    private const string LastNameLongError = "Etternavnet er for langt";
    
    public RegisterUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(EmailIsEmptyError)
            .NotNull().WithMessage(EmailIsEmptyError)
            .Must(email => Regex.IsMatch(email, EmailValidationPattern)).WithMessage(InvalidEmailFormatError);
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(PasswordIsEmptyError)
            .NotNull().WithMessage(PasswordIsEmptyError)
            .Must(password => Regex.IsMatch(password, PasswordValidationPattern)).WithMessage(InvalidPasswordError);

        RuleFor(x => x.ConfirmPassword)
            .Must((model, cp) => model.Password == cp)
            .WithMessage(PasswordsNotEqualError);

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

public sealed class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    private const string PasswordValidationPattern = @"^(?=.*[A-Z])(?=.*[\W_]).{8,}$";
    
    private const string InvalidPasswordError = "Passordet må være minst 8 tegn langt, inneholde minst én stor bokstav og ett spesialtegn";
    private const string PasswordIsEmptyError = "Passord er påkrevd";
    private const string PasswordsNotEqualError = "Passordene er ikke like";
    
    public ChangePasswordDtoValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(PasswordIsEmptyError)
            .NotNull().WithMessage(PasswordIsEmptyError)
            .Must(password => Regex.IsMatch(password, PasswordValidationPattern)).WithMessage(InvalidPasswordError);
        
        RuleFor(x => x.ConfirmPassword)
            .Must((model, cp) => model.NewPassword == cp)
            .WithMessage(PasswordsNotEqualError);
    }
}

public sealed class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    private const string EmailValidationPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    
    private const string InvalidEmailFormatError = "Ugyldig e-postadresse";
    private const string EmailIsEmptyError = "E-postadresse er påkrevd";

    public ResetPasswordDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(EmailIsEmptyError)
            .NotNull().WithMessage(EmailIsEmptyError)
            .Must(email => Regex.IsMatch(email, EmailValidationPattern)).WithMessage(InvalidEmailFormatError);

    }
}

public sealed class ResetPasswordConfirmDtoValidator : AbstractValidator<ResetPasswordConfirmDto>
{
    private const string PasswordValidationPattern = @"^(?=.*[A-Z])(?=.*[\W_]).{8,}$";
    
    private const string InvalidPasswordError = "Passordet må være minst 8 tegn langt, inneholde minst én stor bokstav og ett spesialtegn";
    private const string PasswordIsEmptyError = "Passord er påkrevd";
    private const string PasswordsNotEqualError = "Passordene er ikke like";

    public ResetPasswordConfirmDtoValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(PasswordIsEmptyError)
            .NotNull().WithMessage(PasswordIsEmptyError)
            .Must(password => Regex.IsMatch(password, PasswordValidationPattern)).WithMessage(InvalidPasswordError);
        
        RuleFor(x => x.ConfirmPassword)
            .Must((model, cp) => model.Password == cp)
            .WithMessage(PasswordsNotEqualError);
    }
}