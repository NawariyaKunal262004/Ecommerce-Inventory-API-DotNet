namespace Medical.Application.Validators;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
