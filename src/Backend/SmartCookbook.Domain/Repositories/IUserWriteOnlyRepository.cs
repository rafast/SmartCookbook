using SmartCookbook.Domain.Entities;

namespace SmartCookbook.Domain.Repositories;

public interface IUserWriteOnlyRepository
{
    Task Add(User user);
}
