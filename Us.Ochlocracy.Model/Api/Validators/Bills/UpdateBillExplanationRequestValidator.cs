using FluentValidation;
using Us.Ochlocracy.Model.Api.Requests.Bills;

namespace Us.Ochlocracy.Model.Api.Validators.Bills;

public class UpdateBillExplanationRequestValidator : AbstractValidator<UpdateBillExplanationRequest>
{
    public UpdateBillExplanationRequestValidator()
    {
        RuleFor(x => x.BillExplanationId).GreaterThan(0).WithMessage("BillExplanationId must be greater than 0.");
        RuleFor(x => x.Explanation).NotEmpty().WithMessage("Explanation is required");
    }
}