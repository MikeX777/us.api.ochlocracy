using FluentValidation;
using Us.Ochlocracy.Model.Api.Requests.Bills;

namespace Us.Ochlocracy.Model.Api.Validators.Bills;

public class UpdateBillOpinionRequestValidator : AbstractValidator<UpdateBillOpinionRequest>
{
    public UpdateBillOpinionRequestValidator()
    {
        RuleFor(x => x.BillOpinionId).GreaterThan(0).WithMessage("BillOpinionId must be greater than 0.");
        RuleFor(x => x.Opinion).NotEmpty().WithMessage("Opinion is required");
    }
}