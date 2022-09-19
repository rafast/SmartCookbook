namespace SmartCookbook.Domain.Repositories;

public interface IUnityOfWork
{
    Task Commit();
}
