namespace NBU.Forum.Infrastructure.Articles;

using Application.Articles;
using Domain.ArticleAggregate;
using Configurations;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Polly.Contrib.WaitAndRetry;
using Serilog;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ErrorOr;
using Application.Common.Errors;
using Mapster;
using MapsterMapper;
using Contracts.Responses;

internal sealed class ArticleRepository : IArticleRepository
{
    private readonly AsyncRetryPolicy _asyncRetryPolicy;
    private readonly ForumDbContext _dbContext;
    private readonly ILogger _logger;

    public ArticleRepository(
        ForumDbContext dbContext,
        IOptions<DatabaseRetryPolicyConfiguration> retryPolicyConfiguration,
        ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger.ForContext<ArticleRepository>();
        _asyncRetryPolicy = Policy.Handle<DbUpdateException>()
            .Or<DbUpdateConcurrencyException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                retryPolicyConfiguration.Value.Delay,
                retryPolicyConfiguration.Value.RetryCount));
    }

    public async Task<ErrorOr<GetArticleByIdResponse>> GetByIdAsync(string articleId, CancellationToken cancellationToken = default)
    {
        var result = await this.ExecuteAsync<string, GetArticleByIdResponse>(articleId,
            async () =>
            {
                var article = await _dbContext.Articles
                .Where(x => x.Id == articleId)
                .Select(x => new GetArticleByIdResponse()
                {
                    ArticleId = x.Id,
                    Author = x.AppUser.UserName!,
                    Title = x.Title,
                    Content = x.Content,
                    Comments = x.Comments
                        .Where(x => x.IsApproved && !x.Rejected)
                        .OrderBy(x => x.CreatedAt)
                        .Select(x => new GetCommentResponse()
                        {
                            Author = x.AppUser.UserName!,
                            Content = x.Content,
                            CreatedAt = x.CreatedAt.ToString("yyyy-MM-dd hh:mm tt")
                        })
                        .ToList()
                })
                .SingleOrDefaultAsync(cancellationToken);

                if (article is not null)
                {
                    return article;
                }

                _logger.Error("ArticleId: {ArticleId} not found.",
                    articleId);
                //metrics increment error
                return Errors.Database.NotFound;
            });

        return result;
    }

    public async Task<IEnumerable<TProjection>> GetAsync<TProjection>(CancellationToken cancellationToken = default)
        where TProjection : class
        => await _dbContext.Articles
            .OrderByDescending(x => x.CreatedAt)
            .ProjectToType<TProjection>()
            .ToListAsync(cancellationToken);

    public async Task<ErrorOr<Article>> AddAsync(Article article, CancellationToken cancellationToken = default)
    {
        var result = await this.ExecuteAsync<Article, Article>(article,
            async () =>
            {
                await _dbContext.Articles.AddAsync(article, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return article;
            });

        return result;
    }

    private async Task<ErrorOr<TOut>> ExecuteAsync<TIn, TOut>(
        TIn value,
        Func<Task<ErrorOr<TOut>>> func,
        [CallerMemberName] string methodName = default!)
    {
        try
        {
            // metrics
            return await _asyncRetryPolicy.ExecuteAsync(async () => await func());
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // metrics increment error
            _logger.Error(ex,
                "Concurrency violation occured while saving to the database. MethodName: {MethodName}, Parameter: {@Parameter}",
                methodName,
                value);

            return Errors.Database.Concurrency;
        }
        catch (DbUpdateException ex)
        {
            // metrics increment error
            _logger.Error(ex,
                "Concurrency violation occured while saving to the database. MethodName: {MethodName}, Parameter: {@Parameter}",
                methodName,
                value);

            return Errors.Database.Concurrency;
        }
        catch (OperationCanceledException ex)
        {
            // metrics increment error
            _logger.Error(ex,
                "The operation was canceled. MethodName: {MethodName}, Parameter: {@Parameter}",
                methodName,
                value);

            return Errors.CancellationToken.Cancelled;
        }
        catch (Exception ex)
        {
            // metrics increment error
            _logger.Error(ex,
                "Unexpected error has occured. MethodName: {MethodName}, Parameter: {@Parameter}",
                methodName,
                value);

            return Errors.Database.Unexpected;
        }
    }
}
