namespace NBU.Forum.Web.Controllers;

using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public abstract class MVCController : Controller
{
    protected readonly ISender Sender;

    protected MVCController(ISender sender)
        => Sender = sender;

    protected IActionResult Problem(List<Error> errors)
    {
        if (!errors.Any())
        {
            return this.Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return this.ValidationProblem(errors);
        }

        this.HttpContext.Items["errors"] = errors;

        return this.Problem(errors.First());
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        return this.Problem(statusCode: statusCode,
            title: error.Description);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code,
                error.Description);
        }

        return this.ValidationProblem(modelStateDictionary);
    }
}
