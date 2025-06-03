using LanguageExt;
using MediatR;
using Us.Ochlocracy.Data.Entities;
using Us.Ochlocracy.Data.Repositories;
using Us.Ochlocracy.Interfaces.Repositories;
using Us.Ochlocracy.Model.Api;
using Us.Ochlocracy.Model.Api.Requests.Bills;
using Us.Ochlocracy.Model.Bills;
using Unit = LanguageExt.Unit;

namespace Us.Ochlocracy.Service.V1;

/// <summary>
/// A Command to get paged results of explanations to a bill.
/// </summary>
/// <param name="BillNumber">The number used to identify the bill.</param>
/// <param name="HighestScore">The highest score used to page the results of explanations, only returning explanations of a score less than the supplied value.</param>
public record GetBillExplanations(string BillNumber, int? HighestScore) : IRequest<Either<ApiProblemDetails, IEnumerable<BillExplanation>>> { }
/// <summary>
/// A command to create a explanation to a bill.
/// </summary>
/// <param name="Request">The model containing the data for creating a bill explanation.</param>
public record CreateBillExplanation(CreateBillExplanationRequest Request) : IRequest<Either<ApiProblemDetails, Unit>> { }
/// <summary>
/// A command to update an existing bill explanation.
/// </summary>
/// <param name="Request">The data used to update a bill explanation.</param>
public record UpdateBillExplanation(UpdateBillExplanationRequest Request) : IRequest<Either<ApiProblemDetails, Unit>> { }
/// <summary>
/// A command to update the score of a bill explanation.
/// </summary>
/// <param name="BillExplanationId">The identifier of the bill explanation.</param>
/// <param name="Score">The new score for the bill explanation.</param>
public record UpdateBillExplanationScore(long BillExplanationId, int Score) : IRequest<Either<ApiProblemDetails, Unit>> { }
/// <summary>
/// A command to delete a bill explanation.
/// </summary>
/// <param name="BillExplanationId">The identifier of the bill explanation.</param>
public record DeleteBillExplanation(long BillExplanationId) : IRequest<Either<ApiProblemDetails, Unit>> { }

public record GetUser(string Username) : IRequest<Either<ApiProblemDetails, UserEntity>> { }

/// <summary>
/// A handler to handle commands for the bills.
/// </summary>
public class BillExplanationsHandler(IBillExplanationRepository billExplanations, UserRepository users)
    :
        IRequestHandler<GetBillExplanations, Either<ApiProblemDetails, IEnumerable<BillExplanation>>>,
        IRequestHandler<CreateBillExplanation, Either<ApiProblemDetails, Unit>>,
        IRequestHandler<UpdateBillExplanation, Either<ApiProblemDetails, Unit>>,
        IRequestHandler<UpdateBillExplanationScore, Either<ApiProblemDetails, Unit>>,
        IRequestHandler<DeleteBillExplanation, Either<ApiProblemDetails, Unit>>
{
    public async Task<Either<ApiProblemDetails, IEnumerable<BillExplanation>>> Handle(GetBillExplanations request,
        CancellationToken cancellationToken) =>
        await (
            from br in Common.MapLeft(() => billExplanations.GetBillExplanations(request.BillNumber, request.HighestScore)).ToAsync()
            select br
            );
    
    public async Task<Either<ApiProblemDetails, Unit>> Handle(CreateBillExplanation command,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billExplanations.CreateBillExplanation(
                command.Request.BillNumber,
                command.Request.UserId,
                command.Request.Explanation)).ToAsync()
            select Unit.Default
        );

    public async Task<Either<ApiProblemDetails, Unit>> Handle(UpdateBillExplanation command,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billExplanations.UpdateBillExplanation(
                command.Request.BillExplanationId,
                command.Request.Explanation)).ToAsync()
            select Unit.Default
        );

    public async Task<Either<ApiProblemDetails, Unit>> Handle(UpdateBillExplanationScore request,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billExplanations.UpdateBillExplanationScore(request.BillExplanationId, request.Score)).ToAsync()
            select Unit.Default
        );
    
    public async Task<Either<ApiProblemDetails, Unit>> Handle(DeleteBillExplanation request,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billExplanations.DeleteBillExplanation(request.BillExplanationId)).ToAsync()
            select Unit.Default
        );
}