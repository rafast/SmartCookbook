using Moq;
using SmartCookbook.Application.Services.CurrentUser;

namespace TestsUtility.CurrentUser;
public class CurrentUserBuilder
{
    private static CurrentUserBuilder _instance;
    private readonly Mock<ICurrentUser> _repository;

    private CurrentUserBuilder()
    {
        if (_repository == null)
        {
            _repository = new Mock<ICurrentUser>();
        }
    }

    public static CurrentUserBuilder Instance()
    {
        _instance = new CurrentUserBuilder();
        return _instance;
    }

    public CurrentUserBuilder GetCurrentUser(SmartCookbook.Domain.Entities.User user)
    {
        _repository.Setup(r => r.GetCurrentUser()).ReturnsAsync(user);

        return this;
    }

    public ICurrentUser Build()
    {
        return _repository.Object;
    }
}
