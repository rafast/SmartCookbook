using Bogus;
using SmartCookbook.Domain.Entities;
using TestsUtility.Cryptograph;

namespace TestsUtility.Entities;
public class UserBuilder
{
    public static (User user, string password) Build()
    {
        string password = string.Empty;
        var user = new Faker<User>()
            .RuleFor(c => c.Id, _ => 1)
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Password, f =>
            {
                password = f.Internet.Password();
                return PasswordCryptographBuilder.Instance().Cryptograph(password);
            })
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));


        return (user, password);
    }
}
