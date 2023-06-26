namespace NBU.Forum.Application.PipelineBehaviors;

using Serilog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using ErrorOr;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly ILogger _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        ILogger logger, 
        IEnumerable<IValidator<TRequest>> validators = default!)
    {
        _logger = logger.ForContext(typeof(ValidationBehavior<,>));
        _validators = validators;
    }
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));

        var validationErrors = validationResults.SelectMany(x => x.Errors)
            .Where(x => x is not null)
            .Select(x => Error.Validation(x.PropertyName,
                x.ErrorMessage))
            .ToList();

        if (!validationErrors.Any())
        {
            return await next();
        }

        //validationErrors.ForEach(error => _logger.Error(
        //    "Validation failed for property: {PropertyName}. ErrorMessage: {ErrorMessage}",
        //    error.PropertyName,
        //    error.ErrorMessage));

        return (dynamic)validationErrors;
    }
}
