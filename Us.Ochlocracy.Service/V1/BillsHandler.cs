using LanguageExt;
using LazyCache;
using MediatR;
using Us.Congress.Proxy.Bills;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Api;
using Us.Ochlocracy.Model.Bills;

namespace Us.Ochlocracy.Service.V1
{
    /// <summary>
    /// A command to get the bills in a paged fashion.  
    /// </summary>
    /// <param name="Offset">The offset to use for paging.</param>
    /// <param name="Limit">Limit the number of bills to return.</param>
    public record GetPagedBills(int Offset, int Limit) : IRequest<Either<ApiProblemDetails, BillPartialResponse>> { }
    /// <summary>
    /// A command to get the bills in a paged fashion depending on the supplied congress.
    /// </summary>
    /// <param name="Congress">The congress number to get bills for.</param>
    /// <param name="Offset">The offset to use for paging.</param>
    /// <param name="Limit">Limit the number of bills to return.</param>
    public record GetCongressPagedBills(int Congress, int Offset, int Limit) : IRequest<Either<ApiProblemDetails, BillPartialResponse>> { }
    /// <summary>
    /// A command to get the bills in a paged fashion depending on the supplied congress and bill type.
    /// </summary>
    /// <param name="Congress">The congress number to get bills for.</param>
    /// <param name="BillType">The type of bills to retrieve.</param>
    /// <param name="Offset">The offset to use for paging.</param>
    /// <param name="Limit">Limit the number of bills to return.</param>
    public record GetCongressPagedBillsByType(int Congress, BillType BillType, int Offset, int Limit) : IRequest<Either<ApiProblemDetails, BillPartialResponse>> { }
    /// <summary>
    /// A Command to get the detail of a specific bill.
    /// </summary>
    /// <param name="Congress">The congress number to get bills for.</param>
    /// <param name="BillType">The type of bills to retrieve.</param>
    /// <param name="BillNumber">The number used to identify the bill.</param>
    public record GetBillDetail(int Congress, BillType BillType, string BillNumber) : IRequest<Either<ApiProblemDetails, BillResponse>> { }

    /// <summary>
    /// A handler to handle commands for the bills.
    /// </summary>
    public class BillsHandler(IBillProxy bills) :
    // public class BillsHandler(IBillProxy bills, IAppCache cache) :
        IRequestHandler<GetPagedBills, Either<ApiProblemDetails, BillPartialResponse>>,
        IRequestHandler<GetCongressPagedBills, Either<ApiProblemDetails, BillPartialResponse>>,
        IRequestHandler<GetCongressPagedBillsByType, Either<ApiProblemDetails, BillPartialResponse>>,
        IRequestHandler<GetBillDetail, Either<ApiProblemDetails, BillResponse>>
    {
        /// <summary>
        /// Handler to get paged bills.
        /// </summary>
        /// <param name="request">The request used to retrieved the bills in a paged fashion.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An either of <see cref="ApiProblemDetails"/> or <see cref="BillPartialResponse"/>.</returns>
        public Task<Either<ApiProblemDetails, BillPartialResponse>> Handle(GetPagedBills request, CancellationToken cancellationToken) =>
            from b in Common.MapLeft(() => bills.GetPagedBills(request.Offset, request.Limit, cancellationToken))
            select b;

        /// <summary>
        /// Handler to get paged bills depending on the congress.
        /// </summary>
        /// <param name="request">The request used to retrieve the bills.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An either of <see cref="ApiProblemDetails"/> or <see cref="BillPartialResponse"/>.</returns>
        public Task<Either<ApiProblemDetails, BillPartialResponse>> Handle(GetCongressPagedBills request, CancellationToken cancellationToken) =>
            from b in Common.MapLeft(() => bills.GetCongressPagedBills(request.Congress, request.Offset, request.Limit, cancellationToken))
            select b;

        /// <summary>
        /// Handler to get paged bills depending on the congress and bill type.
        /// </summary>
        /// <param name="request">The request used to retrieve the bills.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An either of <see cref="ApiProblemDetails"/> or <see cref="BillPartialResponse"/>.</returns>
        public Task<Either<ApiProblemDetails, BillPartialResponse>> Handle(GetCongressPagedBillsByType request, CancellationToken cancellationToken) =>
            from b in Common.MapLeft(() => bills.GetCongressPagedBills(request.Congress, request.BillType, request.Offset, request.Limit, cancellationToken))
            select b;

        /// <summary>
        /// Handler to get detail for a specified bill.
        /// </summary>
        /// <param name="request">The request used to retrieve the bill detail.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>An either of <see cref="ApiProblemDetails"/> or <see cref="BillResponse"/>.</returns>
        public Task<Either<ApiProblemDetails, BillResponse>> Handle(GetBillDetail request, CancellationToken cancellationToken) =>
            from b in Common.MapLeft(() => bills.GetBillDetail(request.Congress, request.BillType, request.BillNumber, cancellationToken))
            select b;
    }
}
