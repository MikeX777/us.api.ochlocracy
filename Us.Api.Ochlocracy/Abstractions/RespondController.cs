﻿using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Us.Ochlocracy.Model.Api;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Us.Api.Ochlocracy.Abstractions
{
    /// <summary>
    /// A base controller used to share the respond logic.
    /// </summary>
    public class RespondController : ControllerBase
    {
        /// <summary>
        /// A general respond handler used to return an <see cref="IActionResult"/> from a controller.
        /// </summary>
        /// <typeparam name="O">The "right" type to respond.</typeparam>
        /// <param name="either">The either that can be in either an error or right state.</param>
        /// <param name="mapRight">A handler to be able to override the default functionality of returning an Ok.</param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        protected IActionResult Respond<O>(Either<ApiProblemDetails, O> either, Func<O, IActionResult>? mapRight = null) =>
            either.Match(
                Right: r => mapRight == null ? Ok(r) : mapRight(r),
                Left: e =>
                    e.Status switch
                    {
                        400 => BadRequest(e),
                        404 => NotFound(e),
                        503 => StatusCode(Status503ServiceUnavailable, e),
                        _ => StatusCode(Status500InternalServerError, e),
                    });
    }
}
