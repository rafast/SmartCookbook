using AutoMapper;
using SmartCookbook.Application.Services.Automapper;
using TestsUtility.Hashids;

namespace TestsUtility.Mapper
{
    public class MapperBuilder
    {
        public static IMapper Instance()
        {
            var hashids = HashidsBuilder.Instance().Build();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AutomapperConfig(hashids)));

            return configuration.CreateMapper();
        }
    }
}
