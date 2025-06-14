﻿using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Us.Api.Ochlocracy.Abstractions;
using Us.Ochlocracy.Data.Entities;
using Us.Ochlocracy.Model.Api;
using Us.Ochlocracy.Service.V1;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Us.Api.Ochlocracy.Controllers.V1
{
    /// <summary>
    /// A Controller used to show the basic setup.
    /// </summary>
    /// <remarks>
    /// Constructor for <see cref="DummyController"/>.
    /// </remarks>
    /// <param name="mediator">The mediator instance used to send commands.</param>
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
    public class DummyController(IMediator mediator) : RespondController
    {
        /// <summary>
        /// A general dummy GET request to the <see cref="DummyController"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{String}"/></returns>
        [HttpGet]
        [SwaggerOperation("A dummy action used for general get.")]
        [ProducesResponseType(Status200OK, Type = typeof(IEnumerable<string>))]
        public async Task<IActionResult> GetDummy() =>
            Respond(await mediator.Send(new GetDummyValues()));

        /// <summary>
        /// Dummy endpoint to get bill reactions.
        /// </summary>
        /// <param name="billNumber"></param>
        /// <param name="highestScore"></param>
        /// <returns></returns>
        [HttpGet("billreaction")]
        [SwaggerOperation("Dummy action to get bill reactions from a database.")]
        [ProducesResponseType(Status200OK, Type = typeof(IEnumerable<BillReactionEntity>))]
        public async Task<IActionResult> GetDummyBillReaction(
            [FromQuery] string billNumber,
            [FromQuery] int? highestScore) =>
            Respond(await mediator.Send(new GetBillOpinions(billNumber, highestScore)));
        
        /// <summary>
        /// Dummy endpoint to get users.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("user")]
        [SwaggerOperation("Dummy action to get users from a database.")]
        [ProducesResponseType(Status200OK, Type = typeof(IEnumerable<BillReactionEntity>))]
        public async Task<IActionResult> GetDummyUsers(
            [FromQuery] string username) =>
            Respond(await mediator.Send(new GetUser(username)));
        
        
    }
}
