namespace NBU.Forum.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

public sealed class ErrorsController : Controller
{
    [Route("/error")]
    public IActionResult HandleError()
    {
        var exceptionHandler = this.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return this.Problem(exceptionHandler?.Message ?? "Internal server error",
            statusCode: StatusCodes.Status500InternalServerError);
    }
}
