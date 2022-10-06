using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCookbook.Application.Cryptograph;
using SmartCookbook.Application.Services.CurrentUser;
using SmartCookbook.Application.Services.Token;
using SmartCookbook.Application.UseCases.Login.DoLogin;
using SmartCookbook.Application.UseCases.User.Register;

namespace SmartCookbook.Application;

public static class Bootstraper
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddSaltPassword(services, configuration);
        AddJWTToken(services, configuration);
        AddUseCases(services);
        AddCurrentUser(services);
    
    }

    private static void AddCurrentUser(IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
    }

    private static void AddSaltPassword(IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetRequiredSection("Configurations:SaltPassword");
        services.AddScoped(opt => new PasswordCryptograph(section.Value));
    }

    private static void AddJWTToken(IServiceCollection services, IConfiguration configuration)
    {
        var sectionExpirationTime = configuration.GetRequiredSection("Configurations:TokenExpirationTime");
        var sectionTokenKey = configuration.GetRequiredSection("Configurations:TokenKey");
        services.AddScoped(opt => new TokenController(int.Parse(sectionExpirationTime.Value), sectionTokenKey.Value));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>()
                .AddScoped<ILoginUseCase, LoginUseCase>();
    }
}
