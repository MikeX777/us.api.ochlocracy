using FluentValidation;
using Us.Ochlocracy.Model.Api.Requests.Bills;

namespace Us.Ochlocracy.Model.Api.Validators.Bills;

public class CreateBillOpinionRequestValidator : AbstractValidator<CreateBillOpinionRequest>
{
   public CreateBillOpinionRequestValidator()
   {
      RuleFor(x => x.BillNumber).NotEmpty().WithMessage("BillNumber is required");
      RuleFor(x => x.Opinion).NotEmpty().WithMessage("Opinion is required");
      RuleFor(x => x.UserId).GreaterThan(0).WithMessage("UserId must be a value greater than 0.");
   } 
}