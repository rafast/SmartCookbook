using SmartCookbook.Application.Services.Token;

namespace TestsUtility.Token;

public class TokenControllerBuilder
{
    public static TokenController Instance()
    {
        return new TokenController(1000, "U0dQTjJ4VDQ0SHhvYVRLTUIqayp1bjFDIUhqT3Rj");
    }
}
