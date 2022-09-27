using AutoMapper;
using SmartCookbook.Application.Services.Automapper;

namespace TestsUtility.Mapper
{
    public class MapperBuilder
    {
        public static IMapper Instance()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperConfig>();
            });

            return configuration.CreateMapper();
        }
    }
}
