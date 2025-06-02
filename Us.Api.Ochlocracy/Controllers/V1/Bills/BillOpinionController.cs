using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Us.Api.Ochlocracy.Abstractions;
using Us.Ochlocracy.Model.Api;
using Us.Ochlocracy.Model.Api.Requests.Bills;
using Us.Ochlocracy.Model.Bills;
using Us.Ochlocracy.Service.V1;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Us.Api.Ochlocracy.Controllers.V1.Bills;

/// <summary>
/// A controller pertaining to the bill opinion actions.
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
public class BillOpinionsController(IMediator mediator) : RespondController
{
    /// <summary>
    /// Endpoint to get bill opinions for a specified bill.
    /// </summary>
    /// <param name="billNumber">The string identifier used to denote the bill.</param>
    /// <param name="highestScore">A score used to filter the opinions based on the score they received.</param>
    /// <returns></returns>
    [HttpGet()]
    [SwaggerOperation("Action to get bill opinions from a database by the bill number and score associated with the opinion.")]
    [ProducesResponseType(Status200OK, Type = typeof(IEnumerable<BillOpinion>))]
    public async Task<IActionResult> GetBillOpinionsByBillNumber(
        [FromQuery] string billNumber,
        [FromQuery] int? highestScore) =>
        Respond(await mediator.Send(new GetBillOpinions(billNumber, highestScore)));
    
    /// <summary>
    /// Endpoint to create a new bill opinion.
    /// </summary>
    /// <param name="request">The object used to create the new bill opinion.</param>
    /// <returns></returns>
    [HttpPost()]
    [SwaggerOperation("Action to add bill opinions to the database.")]
    [ProducesResponseType(Status201Created)]
    public async Task<IActionResult> CreateBillOpinion(
        [FromBody] CreateBillOpinionRequest request) =>
    Respond(await mediator.Send(new CreateBillOpinion(request)));

    /// <summary>
    /// Endpoint to update an existing Bill Opinion.
    /// </summary>
    /// <param name="request">The request object containing the data to update a bill opinion.</param>
    /// <returns></returns>
    [HttpPut()]
    [SwaggerOperation("Action to update a specific bill opinion.")]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> UpdateBillOpinion(
        [FromBody] UpdateBillOpinionRequest request) =>
        Respond(await mediator.Send(new UpdateBillOpinion(request)));
    
    /// <summary>
    /// Endpoint to delete a bill opinion by the identifier.
    /// </summary>
    /// <param name="billOpinionId">The identifier of the opinion to delete.</param>
    /// <returns></returns>
    [HttpDelete("{billOpinionId}")]
    [SwaggerOperation("Action to delete a specific bill opinion.")]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> DeleteBillOpinion(
        [FromRoute] long billOpinionId) =>
    Respond(await mediator.Send(new DeleteBillOpinion(billOpinionId)));
}