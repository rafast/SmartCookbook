using AutoMapper;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Domain.Entities;

namespace SmartCookbook.Application.Services.Automapper;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}
