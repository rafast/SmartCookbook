using SmartCookbook.Infrastructure.RepositoryAccess;
using TestsUtility.Entities;

namespace WebApi.Test;
public class ContextSeedInMemory
{
    public static (SmartCookbook.Domain.Entities.User user, string password) Seed(SmartCookbookContext context)
    {
        (var user, string password) = UserBuilder.Build();

        context.Users.Add(user);
        context.SaveChanges();

        return (user, password);
    }
}
