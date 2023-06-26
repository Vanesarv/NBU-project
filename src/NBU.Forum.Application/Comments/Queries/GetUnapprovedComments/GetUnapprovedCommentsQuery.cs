namespace NBU.Forum.Application.Comments.Queries.GetUnapprovedComments;

using MediatR;
using Contracts.Responses;

public sealed class GetUnapprovedCommentsQuery : IRequest<IEnumerable<GetUnapprovedCommentResponse>>
{ }