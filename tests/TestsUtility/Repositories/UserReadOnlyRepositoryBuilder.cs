using Moq;
using SmartCookbook.Domain.Repositories;

namespace TestsUtility.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private static UserReadOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserReadOnlyRepository> _repository;

    private UserReadOnlyRepositoryBuilder()
    {
        if (_repository == null)
        {
            _repository = new Mock<IUserReadOnlyRepository>();
        }
    }

    public static UserReadOnlyRepositoryBuilder Instance()
    {
        _instance = new UserReadOnlyRepositoryBuilder();
        return _instance;
    }

    public UserReadOnlyRepositoryBuilder IsEmailInUse(string email)
    {
        if (!string.IsNullOrEmpty(email))
            _repository.Setup(i => i.IsEmailInUse(email)).ReturnsAsync(true);

        return this;
    }

    public IUserReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
