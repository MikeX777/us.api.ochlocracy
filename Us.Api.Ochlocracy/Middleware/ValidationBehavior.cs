using FluentValidation;
using MediatR;

namespace Us.Api.Ochlocracy.Middleware
{
    /// <summary>
    /// A pipeline used to handle verification.
    /// </summary>
    /// <typeparam name="Request"></typeparam>
    /// <typeparam name="Response"></typeparam>
    public class ValidationBehavior<Request, Response> : IPipelineBehavior<Request, Response> where Request : IRequest<Response>
    {
        private readonly IEnumerable<IValidator<Request>> validators;

        /// <summary>
        /// The constructor for <see cref="ValidationBehavior{Request, Response}"/>.
        /// </summary>
        /// <param name="validators"></param>
        public ValidationBehavior(IEnumerable<IValidator<Request>> validators) => this.validators = validators;

        /// <summary>
        /// The handler for the pipeline.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public Task<Response> Handle(Request request, RequestHandlerDelegate<Response> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<Request>(request);
            var failures = validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return next();
        }
    }
}
