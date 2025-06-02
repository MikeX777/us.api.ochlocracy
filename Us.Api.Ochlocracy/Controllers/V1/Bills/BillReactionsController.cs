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
public class BillExplanationsController(IMediator mediator) : RespondController
{
    /// <summary>
    /// Endpoint to get bill explanation for a specified bill.
    /// </summary>
    /// <param name="billNumber">The string identifier used to denote the bill.</param>
    /// <param name="highestScore">A score used to filter the explantion based on the score they received.</param>
    /// <returns></returns>
    [HttpGet()]
    [SwaggerOperation("Action to get bill explanations from a database by the bill number and score associated with the explanation.")]
    [ProducesResponseType(Status200OK, Type = typeof(IEnumerable<BillExplanation>))]
    public async Task<IActionResult> GetBillExplanationsByBillNumber(
        [FromQuery] string billNumber,
        [FromQuery] int? highestScore) =>
        Respond(await mediator.Send(new GetBillExplanations(billNumber, highestScore)));
    
    /// <summary>
    /// Endpoint to create a new bill explanation.
    /// </summary>
    /// <param name="request">The object used to create the new bill explanation.</param>
    /// <returns></returns>
    [HttpPost()]
    [SwaggerOperation("Action to add bill explanations to the database.")]
    [ProducesResponseType(Status201Created)]
    public async Task<IActionResult> CreateBillExplanation(
        [FromBody] CreateBillExplanationRequest request) =>
    Respond(await mediator.Send(new CreateBillExplanation(request)));

    /// <summary>
    /// Endpoint to update an existing Bill Explanation.
    /// </summary>
    /// <param name="request">The request object containing the data to update a bill explanation.</param>
    /// <returns></returns>
    [HttpPut()]
    [SwaggerOperation("Action to update a specific bill explanation.")]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> UpdateBillExplanation(
        [FromBody] UpdateBillExplanationRequest request) =>
        Respond(await mediator.Send(new UpdateBillExplanation(request)));
    
    /// <summary>
    /// Endpoint to delete a bill explanation by the identifier.
    /// </summary>
    /// <param name="billExplanationId">The identifier of the explanation to delete.</param>
    /// <returns></returns>
    [HttpDelete("{billExplanationId}")]
    [SwaggerOperation("Action to delete a specific bill explanation.")]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> DeleteBillExplanation(
        [FromRoute] long billExplanationId) =>
    Respond(await mediator.Send(new DeleteBillExplanation(billExplanationId)));
}