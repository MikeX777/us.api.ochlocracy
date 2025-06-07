using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Us.Api.Ochlocracy.Abstractions;
using Us.Ochlocracy.Model;
using Us.Ochlocracy.Model.Api;
using Us.Ochlocracy.Model.Bills;
using Us.Ochlocracy.Service.V1;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Us.Api.Ochlocracy.Controllers.V1.Bills;

/// <summary>
/// A controller pertaining to the bill explanation actions.
/// </summary>
/// <param name="mediator">The mediator service to handle the methods that are called.</param>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[ProducesResponseType(Status401Unauthorized, Type = typeof(ApiProblemDetails))]
[ProducesResponseType(Status403Forbidden, Type = typeof(ApiProblemDetails))]
[ProducesResponseType(Status404NotFound, Type = typeof(ApiProblemDetails))]
[ProducesResponseType(Status503ServiceUnavailable, Type = typeof(ApiProblemDetails))]
[ProducesResponseType(Status504GatewayTimeout, Type = typeof(ApiProblemDetails))]
[ProducesResponseType(Status500InternalServerError, Type = typeof(ApiProblemDetails))]
[Consumes("application/json")]
[Produces("application/json")]
public class BillsController(IMediator mediator) : RespondController
{

    /// <summary>
    /// An endpoint to retrieve a paged response of bills. Based off the Congress API.
    /// </summary>
    /// <param name="offset">The number of bills to skip while retrieving bills.</param>
    /// <param name="limit">The number of bills to retrieve.</param>
    /// <returns></returns>
    [HttpGet()]
    [SwaggerOperation("Retrieves a paged response of the available bills.")]
    [ProducesResponseType(Status200OK, Type = typeof(BillPartialResponse))]
    public async Task<IActionResult> RetrievePagedBills(
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10) =>
        Respond(await mediator.Send(new GetPagedBills(offset, limit)));
    
    /// <summary>
    /// Gets a paged response of bills of a specified congress.
    /// </summary>
    /// <param name="congressId">The number used to identify the congress.</param>
    /// <param name="offset">The offset used for paging the bills.</param>
    /// <param name="limit">The number of bills to return.</param>
    /// <returns></returns>
    [HttpGet("{congressId:int}")]
    [SwaggerOperation("Retrieves a paged response of the bills of a given congress.")]
    [ProducesResponseType(Status200OK, Type = typeof(BillPartialResponse))]
    public async Task<IActionResult> RetrieveBillsByCongressId(
        [FromRoute] int congressId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10) =>
        Respond(await mediator.Send(new GetCongressPagedBills(congressId, offset, limit)));

    /// <summary>
    /// Gets a paged response of bills of a specified congress, filtered by bill type.
    /// </summary>
    /// <param name="congressId">The number used to identify the congress.</param>
    /// <param name="billType">The type of bill to retrieve.</param>
    /// <param name="offset">The offset used for paging the bills.</param>
    /// <param name="limit">The number of bills to return.</param>
    /// <returns></returns>
    [HttpGet("{congressId}/{billType}")]
    [SwaggerOperation("Retrieves a paged response of the bills of a given congress, filtered by bill type.")]
    [ProducesResponseType(Status200OK, Type = typeof(BillPartialResponse))]
    public async Task<IActionResult> RetrieveBillsByCongressIdAndBillType(
        [FromRoute] int congressId,
        [FromRoute] BillType billType,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 10) =>
        Respond(await mediator.Send(new GetCongressPagedBillsByType(congressId, billType, offset, limit)));

    /// <summary>
    /// Retrieves the bill detail using the congress number, bill type, and bill number.
    /// </summary>
    /// <param name="congressId">The identifier used to specify the congress.</param>
    /// <param name="billType">The type of bill to retrieve.</param>
    /// <param name="billNumber">The number used to identify the specific bill.</param>
    /// <returns></returns>
    [HttpGet("{congressId}/{billType}/{billNumber}")]
    [SwaggerOperation(
        "Retrieves the bill detail of the specified bill using the congress, bill type, and bill number.")]
    [ProducesResponseType(Status200OK, Type = typeof(BillResponse))]
    public async Task<IActionResult> RetrieveBillDetail(
        [FromRoute] int congressId,
        [FromRoute] BillType billType,
        [FromRoute] string billNumber) =>
        Respond(await mediator.Send(new GetBillDetail(congressId, billType, billNumber)));
}