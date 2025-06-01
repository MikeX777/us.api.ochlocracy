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
/// A Command to get paged results of reactions to a bill.
/// </summary>
/// <param name="BillNumber">The number used to identify the bill.</param>
/// <param name="HighestScore">The highest score used to page the results of reactions, only returning reactions of a score less than the supplied value.</param>
public record GetBillReactions(string BillNumber, int? HighestScore) : IRequest<Either<ApiProblemDetails, IEnumerable<BillReaction>>> { }
/// <summary>
/// A command to create a reaction to a bill.
/// </summary>
/// <param name="Request">The model containing the data for creating a bill reaction.</param>
public record CreateBillReaction(CreateBillReactionRequest Request) : IRequest<Either<ApiProblemDetails, Unit>> { }
/// <summary>
/// A command to update an existing bill reaction.
/// </summary>
/// <param name="Request">The data used to update a bill reaction.</param>
public record UpdateBillReaction(UpdateBillReactionRequest Request) : IRequest<Either<ApiProblemDetails, Unit>> { }
/// <summary>
/// A command to update the score of a bill reaction.`
/// </summary>
/// <param name="BillReactionId">The identifier of the bill reaction.</param>
/// <param name="Score">The new score for the bill reaction.</param>
public record UpdateBillReactionScore(long BillReactionId, int Score) : IRequest<Either<ApiProblemDetails, Unit>> { }
/// <summary>
/// A command to delete a bill reaction.
/// </summary>
/// <param name="BillReactionId">The identifier of the bill reaction.</param>
public record DeleteBillReaction(long BillReactionId) : IRequest<Either<ApiProblemDetails, Unit>> { }

public record GetUser(string Username) : IRequest<Either<ApiProblemDetails, UserEntity>> { }

/// <summary>
/// A handler to handle commands for the bills.
/// </summary>
public class BillReactionsHandler(IBillReactionRepository billReactions, UserRepository users)
    :
        IRequestHandler<GetBillReactions, Either<ApiProblemDetails, IEnumerable<BillReaction>>>,
        IRequestHandler<CreateBillReaction, Either<ApiProblemDetails, Unit>>,
        IRequestHandler<UpdateBillReaction, Either<ApiProblemDetails, Unit>>,
        IRequestHandler<UpdateBillReactionScore, Either<ApiProblemDetails, Unit>>,
        IRequestHandler<DeleteBillReaction, Either<ApiProblemDetails, Unit>>
{
    public async Task<Either<ApiProblemDetails, IEnumerable<BillReaction>>> Handle(GetBillReactions request,
        CancellationToken cancellationToken) =>
        await (
            from br in Common.MapLeft(() => billReactions.GetBillReactions(request.BillNumber, request.HighestScore)).ToAsync()
            select br
            );
    
    public async Task<Either<ApiProblemDetails, Unit>> Handle(CreateBillReaction command,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billReactions.CreateBillReaction(
                command.Request.BillNumber,
                command.Request.UserId,
                command.Request.Explanation,
                command.Request.Opinion)).ToAsync()
            select Unit.Default
        );

    public async Task<Either<ApiProblemDetails, Unit>> Handle(UpdateBillReaction command,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billReactions.UpdateBillReaction(
                command.Request.BillReactionId,
                command.Request.Explanation,
                command.Request.Opinion)).ToAsync()
            select Unit.Default
        );

    public async Task<Either<ApiProblemDetails, Unit>> Handle(UpdateBillReactionScore request,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billReactions.UpdateBillReactionScore(request.BillReactionId, request.Score)).ToAsync()
            select Unit.Default
        );
    
    public async Task<Either<ApiProblemDetails, Unit>> Handle(DeleteBillReaction request,
        CancellationToken cancellationToken) =>
        await (
            from _ in Common.MapLeft(() => billReactions.DeleteBillReaction(request.BillReactionId)).ToAsync()
            select Unit.Default
        );
}