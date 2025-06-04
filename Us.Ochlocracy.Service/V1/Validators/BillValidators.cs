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

public class GetCongressPagedBillsValidator : AbstractValidator<GetCongressPagedBills>
{
    public GetCongressPagedBillsValidator()
    {
        RuleFor(x => x.Limit).GreaterThan(0).WithMessage("Limit must be greater than 0.");
        RuleFor(x => x.Limit).LessThanOrEqualTo(100).WithMessage("Limit must be less than or equal to 100.");
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).WithMessage("Offset must be a positive number.");
        RuleFor(x => x.Congress).GreaterThan(0).WithMessage("Congress Number must be greater than 0.");
    }
}

public class GetCongressPagedBillByTypeValidator : AbstractValidator<GetCongressPagedBillsByType>
{
    public GetCongressPagedBillByTypeValidator()
    {
        RuleFor(x => x.Limit).GreaterThan(0).WithMessage("Limit must be greater than 0.");
        RuleFor(x => x.Limit).LessThanOrEqualTo(100).WithMessage("Limit must be less than or equal to 100.");
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0).WithMessage("Offset must be a positive number.");
        RuleFor(x => x.Congress).GreaterThan(0).WithMessage("Congress Number must be greater than 0.");
    } 
}

public class GetBillDetailValidator : AbstractValidator<GetBillDetail>
{
    public GetBillDetailValidator()
    {
        RuleFor(x => x.Congress).GreaterThan(0).WithMessage("Congress Number must be greater than 0.");
        RuleFor(x => x.BillNumber).NotEmpty().WithMessage("Bill number must not be empty.");
    }
}