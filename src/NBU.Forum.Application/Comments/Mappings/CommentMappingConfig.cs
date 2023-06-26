namespace NBU.Forum.Application.Comments.Mappings;

using Mapster;
using Contracts.Responses;
using Domain.CommentAggregate;

public sealed class CommentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Comment, GetCommentResponse>()
            .Map(dest => dest.Author,
                src => src.AppUser.UserName)
            .Map(dest => dest.Content,
                src => src.Content)
            .Map(dest => dest.CreatedAt,
                src => src.CreatedAt.ToString("yyyy-MM-dd hh:mm tt"));
    }
}
