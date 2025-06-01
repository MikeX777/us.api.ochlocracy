using FluentValidation;
using Us.Ochlocracy.Model.Api.Requests.Bills;

namespace Us.Ochlocracy.Model.Api.Validators.Bills;

public class UpdateBillReactionRequestValidator : AbstractValidator<UpdateBillReactionRequest>
{
    public UpdateBillReactionRequestValidator()
    {
        RuleFor(x => x.BillReactionId).GreaterThan(0).WithMessage("BillReactionId must be greater than 0.");
        RuleFor(x => x.Explanation).NotEmpty().WithMessage("Explanation is required");
        RuleFor(x => x.Opinion).NotEmpty().WithMessage("Opinion is required");
    }
}