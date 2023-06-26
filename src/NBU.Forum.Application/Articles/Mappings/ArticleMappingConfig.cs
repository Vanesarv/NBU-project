namespace NBU.Forum.Application.Articles.Mappings;

using Mapster;
using Contracts.Responses;
using Domain.ArticleAggregate;

public sealed class ArticleMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Article, GetArticleResponse>()
            .Map(dest => dest.ArticleId,
                src => src.Id)
            .Map(dest => dest.Title,
                src => src.Title)
            .Map(dest => dest.Author,
                src => src.AppUser.UserName);

        config.NewConfig<Article, GetArticleByIdResponse>()
            .Map(dest => dest.ArticleId,
                src => src.Id)
            .Map(dest => dest.Title,
                src => src.Title)
            .Map(dest => dest.Content,
                src => src.Content)
            .Map(dest => dest.Author,
                src => src.AppUser.UserName);
    }
}
