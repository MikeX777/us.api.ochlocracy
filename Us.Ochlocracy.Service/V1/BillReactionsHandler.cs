using LanguageExt;
using LazyCache;
using MediatR;
using Us.Congress.Proxy.Bills;
using Us.Ochlocracy.Data.Entities;
using Us.Ochlocracy.Data.Repositories;
using Us.Ochlocracy.Interfaces.Repositories;
using Us.Ochlocracy.Model.Api;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Service.V1;

/// <summary>
/// A Command to get paged results of reactions to a bill.
/// </summary>
/// <param name="BillNumber">The number used to identify the bill.</param>
/// <param name="HighestScore">A highest score used to page the results of reactions, only returning reactions of a score less than the supplied value.</param>
public record GetBillReactions(string BillNumber, int? HighestScore) : IRequest<Either<ApiProblemDetails, IEnumerable<BillReaction>>> { }

public record GetUser(string Username) : IRequest<Either<ApiProblemDetails, UserEntity>> { }

/// <summary>
/// A handler to handle commands for the bills.
/// </summary>
public class BillReactionsHandler :
    IRequestHandler<GetBillReactions, Either<ApiProblemDetails, IEnumerable<BillReaction>>>
{
    private readonly BillReactionRepository billReactions;
    private readonly UserRepository users;

    public BillReactionsHandler(BillReactionRepository billReactions, UserRepository users) =>
        (this.billReactions, this.users) = (billReactions, users);

    public async Task<Either<ApiProblemDetails, IEnumerable<BillReaction>>> Handle(GetBillReactions request,
        CancellationToken cancellationToken) =>
        await (
            from br in Common.MapLeft(() => billReactions.GetBillReactions(request.BillNumber, request.HighestScore)).ToAsync()
            select br
            );
}