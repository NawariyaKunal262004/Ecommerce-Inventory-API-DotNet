namespace Medical.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).ApplyPasswordRules();
        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role => AuthConstants.ValidRoles.Contains(role))
            .WithMessage("Role must be Admin or Biller.");
        RuleFor(x => x.OrganizationId).NotEmpty();
    }
}
