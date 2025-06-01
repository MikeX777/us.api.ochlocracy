using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Us.Api.Ochlocracy.Abstractions;
using Us.Ochlocracy.Data.Entities;
using Us.Ochlocracy.Model.Api;
using Us.Ochlocracy.Model.Api.Requests.Bills;
using Us.Ochlocracy.Service.V1;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Us.Api.Ochlocracy.Controllers.V1.Bills;

/// <summary>
/// A controller pertaining to the bill reaction actions.
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
public class BillReactionsController(IMediator mediator) : RespondController
{
    /// <summary>
    /// Endpoint to get bill reactions for a specified bill.
    /// </summary>
    /// <param name="billNumber">The string identifier used to denote the bill.</param>
    /// <param name="highestScore">A score used to filter the reactions based on the score they received.</param>
    /// <returns></returns>
    [HttpGet()]
    [SwaggerOperation("Action to get bill reactions from a database by the bill number and score associated with the reaction.")]
    [ProducesResponseType(Status200OK, Type = typeof(IEnumerable<BillReactionEntity>))]
    public async Task<IActionResult> GetBillReactionsByBillNumber(
        [FromQuery] string billNumber,
        [FromQuery] int? highestScore) =>
        Respond(await mediator.Send(new GetBillReactions(billNumber, highestScore)));
    
    /// <summary>
    /// Endpoint to create a new bill reaction.
    /// </summary>
    /// <param name="request">The object used to create the new bill reaction.</param>
    /// <returns></returns>
    [HttpPost()]
    [SwaggerOperation("Action to add bill reactions to the database.")]
    [ProducesResponseType(Status201Created)]
    public async Task<IActionResult> CreateBillReaction(
        [FromBody] CreateBillReactionRequest request) =>
    Respond(await mediator.Send(new CreateBillReaction(request)));

    /// <summary>
    /// Endpoint to update an existing Bill Reaction.
    /// </summary>
    /// <param name="request">The request object containing the data to update a bill reaction.</param>
    /// <returns></returns>
    [HttpPut()]
    [SwaggerOperation("Action to update a specific bill reaction.")]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> UpdateBillReaction(
        [FromBody] UpdateBillReactionRequest request) =>
        Respond(await mediator.Send(new UpdateBillReaction(request)));
    
    /// <summary>
    /// Endpoint to delete a bill reaction by the identifier.
    /// </summary>
    /// <param name="billReactionId">The identifier of the reaction to delete.</param>
    /// <returns></returns>
    [HttpDelete("{billReactionId}")]
    [SwaggerOperation("Action to delete a specific bill reaction.")]
    [ProducesResponseType(Status204NoContent)]
    public async Task<IActionResult> DeleteBillReaction(
        [FromRoute] long billReactionId) =>
    Respond(await mediator.Send(new DeleteBillReaction(billReactionId)));
}