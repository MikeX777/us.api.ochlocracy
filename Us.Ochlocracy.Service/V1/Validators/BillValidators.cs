using FluentValidation;
using Us.Ochlocracy.Service.V1;

namespace Us.Ochlocracy.Service.V1.Validators;

public class GetPagedBillsValidator : AbstractValidator<GetPagedBills>
{
    public GetPagedBillsValidator()
    {
        RuleFor(x => x.Limit).GreaterThan(0).WithMessage("Limit must be greater than 0.");
        RuleFor(x => x.Limit).LessThanOrEqualTo(100).WithMessage("Limit must be less than or equal to 100.");
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).WithMessage("Offset must be a positive number.");
    }
}