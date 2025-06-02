using LanguageExt;
using MediatR;
using Us.Ochlocracy.Data.Repositories;
using Us.Ochlocracy.Interfaces.Repositories;
using Us.Ochlocracy.Model.Api;
using Us.Ochlocracy.Model.Api.Requests.Bills;
using Us.Ochlocracy.Model.Bills;
using Unit = LanguageExt.Unit;

namespace Us.Ochlocracy.Service.V1;

/// <summary>
/// A Command to get paged results of opinions to a bill.
/// </summary>
/// <param name="BillNumber">The number used to identify the bill.</param>
/// <param name="HighestScore">The highest score used to page the results of opinions, only returning opinions of a score less than the supplied value.</param>
public record GetBillOpinions(string BillNumber, int? HighestScore) : IRequest<Either<ApiProblemDetails, IEnumerable<BillOpinion>>> { }
/// <summary>
/// A command to create a opinion to a bill.
/// </summary>
/// <param name="Request">The model containing the data for creating a bill opinion.</param>
public record CreateBillOpinion(CreateBillOpinionRequest Request) : IRequest<Either<ApiProblemDetails, Unit>> { }
/// <summary>
/// A command to update an existing bill opinion.
/// </summary>
/// <param name="Request">The data used to update a bill opinion.</param>
public record UpdateBillOpinion(UpdateBillOpinionRequest Request) : IRequest<Either<ApiProblemDetails, Unit>> { }
/// <summary>
/// A command to update the score of a bill opinion.
/// </summary>
/// <param name="BillOpinionId">The identifier of the bill opinion.</param>
/// <param name="Score">The new score for the bill opinion.</param>
public record UpdateBillOpinionScore(long BillOpinionId, int Score) : IRequest<Either<ApiProblemDetails, Unit>> { }
/// <summary>
/// A command to delete a bill opinion.
/// </summary>
/// <param name="BillOpinionId">The identifier of the bill opinion.</param>
public record DeleteBillOpinion(long BillOpinionId) : IRequest<Either<ApiProblemDetails, Unit>> { }

/// <summary>
/// A handler to handle commands for the bills.
/// </summary>
public class BillOpinionHandler(IBillOpinionRepository billOpinion, UserRepository users)
    :
        IRequestHandler<GetBillOpinions, Either<ApiProblemDetails, IEnumerable<BillOpinion>>>,
        IRequestHandler<CreateBillOpinion, Either<ApiProblemDetails, Unit>>,
        IRequestHandler<UpdateBillOpinion, Either<ApiProblemDetails, Unit>>,
        IRequestHandler<UpdateBillOpinionScore, Either<ApiProblemDetails, Unit>>,
        IRequestHandler<DeleteBillOpinion, Either<ApiProblemDetails, Unit>>
{
    public async Task<Either<ApiProblemDetails, IEnumerable<BillOpinion>>> Handle(GetBillOpinions request,
        CancellationToken cancellationToken) =>
        await (
            from br in Common.MapLeft(() => billOpinion.GetBillOpinions(request.BillNumber, request.HighestScore)).ToAsync()
            select br
            );
    
    public async Task<Either<ApiProblemDetails, Unit>> Handle(CreateBillOpinion command,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billOpinion.CreateBillOpinion(
                command.Request.BillNumber,
                command.Request.UserId,
                command.Request.Opinion)).ToAsync()
            select Unit.Default
        );

    public async Task<Either<ApiProblemDetails, Unit>> Handle(UpdateBillOpinion command,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billOpinion.UpdateBillOpinion(
                command.Request.BillOpinionId,
                command.Request.Opinion)).ToAsync()
            select Unit.Default
        );

    public async Task<Either<ApiProblemDetails, Unit>> Handle(UpdateBillOpinionScore request,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billOpinion.UpdateBillOpinionScore(request.BillOpinionId, request.Score)).ToAsync()
            select Unit.Default
        );
    
    public async Task<Either<ApiProblemDetails, Unit>> Handle(DeleteBillOpinion request,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billOpinion.DeleteBillOpinion(request.BillOpinionId)).ToAsync()
            select Unit.Default
        );
}