namespace Medical.Application.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Role)
            .Must(role => string.IsNullOrWhiteSpace(role) || AuthConstants.ValidRoles.Contains(role!))
            .WithMessage("Role must be Admin or Biller.");
    }
}
