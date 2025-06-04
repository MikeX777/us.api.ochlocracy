using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Us.Api.Ochlocracy.Abstractions;
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
}