using Bogus;
using SmartCookbook.Comunicacao.Request;

namespace TestsUtility.Requests;
public class ChangePasswordRequestBuilder
{
    public static ChangePasswordRequestJson Build(int passwordLength = 10)
    {
        return new Faker<ChangePasswordRequestJson>()
            .RuleFor(c => c.OldPassword, f => f.Internet.Password(10))
            .RuleFor(c => c.NewPassword, f => f.Internet.Password(passwordLength));
    }
}
