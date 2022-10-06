using SmartCookbook.Domain.Entities;

namespace SmartCookbook.Application.Services.CurrentUser;
public interface ICurrentUser
{
    Task<User> GetCurrentUser();
}
