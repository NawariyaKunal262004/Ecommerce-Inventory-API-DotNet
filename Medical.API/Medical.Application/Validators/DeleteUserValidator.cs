namespace Medical.Application.Validators;

public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
