namespace NBU.Forum.Web.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Contracts.Requests;
using Application.Articles.Commands.CreateArticle;
using Application.Articles.Queries.GetArticleById;
using System.Security.Claims;

public sealed class ArticleController : MVCController
{
    public ArticleController(ISender sender)
        : base(sender) { }

    public IActionResult Create()
        => View();

    public async Task<IActionResult> Details(string articleId)
    {
        var query = new GetArticleByIdQuery(articleId);
        var result = await Sender.Send(query);

        return result.IsError
            ? this.Problem(result.Errors)
            : View(result.Value);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateArticleRequest model)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new CreateArticleCommand(
            model.Title,
            model.Content,
            userId!);

        var result = await Sender.Send(command);

        return result.IsError
            ? this.Problem(result.Errors)
            : RedirectToAction(controllerName: "Article", actionName: "Details", routeValues: new { articleId = result.Value.ArticleId });
    }
}
