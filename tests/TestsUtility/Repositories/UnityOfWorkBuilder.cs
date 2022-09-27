using Moq;
using SmartCookbook.Domain.Repositories;

namespace TestsUtility.Repositories;

public class UnityOfWorkBuilder
{
    private static UnityOfWorkBuilder _instance;
    private readonly Mock<IUnityOfWork> _repository;

    private UnityOfWorkBuilder()
    {
        if (_repository == null)
        {
            _repository = new Mock<IUnityOfWork>();
        }
    }

    public static UnityOfWorkBuilder Instance()
    {
        _instance = new UnityOfWorkBuilder();
        return _instance;
    }

    public IUnityOfWork Build()
    {
        return _repository.Object;
    }
}
