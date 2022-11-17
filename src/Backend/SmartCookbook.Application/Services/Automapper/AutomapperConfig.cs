using AutoMapper;
using HashidsNet;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Comunicacao.Response;
using SmartCookbook.Domain.Entities;

namespace SmartCookbook.Application.Services.Automapper;

public class AutomapperConfig : Profile
{
    private readonly IHashids _hashids;
    public AutomapperConfig(IHashids hashids)
    {
        _hashids = hashids;

        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<IngredientRequestJson, Ingredient>();
        CreateMap<RecipeRequestJson, Recipe>();
    }

    private void EntityToResponse()
    {
        CreateMap<Recipe, RecipeResponseJson>()
            .ForMember(dest => dest.Id, config => config.MapFrom(origin => _hashids.EncodeLong(origin.Id)));

        CreateMap<Ingredient, IngredientResponseJson>()
            .ForMember(dest => dest.Id, config => config.MapFrom(origin => _hashids.EncodeLong(origin.Id)));
    }
}
