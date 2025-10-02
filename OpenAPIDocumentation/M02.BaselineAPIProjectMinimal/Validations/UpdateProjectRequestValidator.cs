using FluentValidation;
using M02.BaselineAPIProjectMinimal.Requests;

namespace M02.BaselineAPIProjectMinimal.Validations;

public class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.ExpectedStartDate).GreaterThan(DateTime.MinValue);
    }
}

