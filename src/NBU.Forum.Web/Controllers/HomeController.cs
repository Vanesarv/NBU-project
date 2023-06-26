namespace NBU.Forum.Web.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Articles.Queries.GetArticles;

public sealed class HomeController : MVCController
{
    public HomeController(ISender sender)
        : base(sender) {}

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var query = new GetArticlesQuery();
        var result = await Sender.Send(query);

        return View(result);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}