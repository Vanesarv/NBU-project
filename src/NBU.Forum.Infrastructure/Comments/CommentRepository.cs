namespace NBU.Forum.Infrastructure.Comments;

using Application.Common.Errors;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Application.Comments;
using Domain.CommentAggregate;
using Configurations;
using Persistence;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using Serilog;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Responses;

internal sealed class CommentRepository : ICommentRepository
{
    private readonly AsyncRetryPolicy _asyncRetryPolicy;
    private readonly ForumDbContext _dbContext;
    private readonly ILogger _logger;

    public CommentRepository(
        ForumDbContext dbContext,
        IOptions<DatabaseRetryPolicyConfiguration> retryPolicyConfiguration,
        ILogger logger)
    {
        _dbContext = dbContext;
        _logger = logger.ForContext<CommentRepository>();
        _asyncRetryPolicy = Policy.Handle<DbUpdateException>()
            .Or<DbUpdateConcurrencyException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(
                retryPolicyConfiguration.Value.Delay,
                retryPolicyConfiguration.Value.RetryCount));
    }

    public async Task<IEnumerable<GetUnapprovedCommentResponse>> GetAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Comments
            .Where(x => !x.IsApproved && !x.Rejected)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new GetUnapprovedCommentResponse()
            {
                CommentId = x.Id,
                ArticleId = x.ArticleId,
                Author = x.AppUser.UserName!,
                Content = x.Content
            })
            .ToListAsync(cancellationToken);

    public async Task<ErrorOr<Comment>> ApproveCommentAsync(string commentId, CancellationToken cancellationToken = default)
    {
        var result = await this.ExecuteAsync<string, Comment>(commentId,
            async () =>
            {
                var comment = await _dbContext.Comments.SingleOrDefaultAsync(c => c.Id == commentId, cancellationToken);

                if (comment is null)
                {
                    return Errors.Database.NotFound;
                }

                comment.SetApprovedStatus();

                _dbContext.Comments.Update(comment);
                await _dbContext.SaveChangesAsync();

                return comment;
            });

        return result;
    }

    public async Task<ErrorOr<Comment>> RejectCommentAsync(string commentId, CancellationToken cancellationToken = default)
    {
        var result = await this.ExecuteAsync<string, Comment>(commentId,
            async () =>
            {
                var comment = await _dbContext.Comments.SingleOrDefaultAsync(c => c.Id == commentId, cancellationToken);

                if (comment is null)
                {
                    return Errors.Database.NotFound;
                }

                comment.SetRejectedStatus();

                _dbContext.Comments.Update(comment);
                await _dbContext.SaveChangesAsync();

                return comment;
            });

        return result;
    }

    public async Task<ErrorOr<Comment>> AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        var result = await this.ExecuteAsync<Comment, Comment>(comment,
            async () =>
            {
                await _dbContext.Comments.AddAsync(comment, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return comment;
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
