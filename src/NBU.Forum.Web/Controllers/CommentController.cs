namespace NBU.Forum.Web.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Contracts.Requests;
using Application.Comments.Queries.GetUnapprovedComments;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Application.Comments.Commands.ApproveComment;
using Application.Comments.Commands.RejectComment;
using Application.Comments.Commands.CreateComment;

[Authorize]
public sealed class CommentController : MVCController
{
    public CommentController(ISender sender)
    : base(sender) { }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCommentRequest request)
    {
        var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new CreateCommentCommand(
            request.ArticleId,
            userId!,
            request.Content);

        var result = await Sender.Send(command);

        return result.IsError
            ? this.Problem(result.Errors)
            : RedirectToAction(controllerName: "Article", actionName: "Details", routeValues: new { articleId = request.ArticleId });
    }

    [HttpGet]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> Index()
    {
        var query = new GetUnapprovedCommentsQuery();

        var result = await Sender.Send(query);

        return View(result);
    }

    [HttpPost]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> Approve(string commentId)
    {
        var command = new ApproveCommentCommand(commentId);

        var result = await Sender.Send(command);

        return result.IsError
            ? this.Problem(result.Errors)
            : RedirectToAction(controllerName: "Comment", actionName: "Index");
    }

    [HttpPost]
    [Authorize(Roles = "Moderator")]
    public async Task<IActionResult> Reject(string commentId)
    {
        var command = new RejectCommentCommand(commentId);

        var result = await Sender.Send(command);

        return result.IsError
            ? this.Problem(result.Errors)
            : RedirectToAction(controllerName: "Comment", actionName: "Index");
    }
}
