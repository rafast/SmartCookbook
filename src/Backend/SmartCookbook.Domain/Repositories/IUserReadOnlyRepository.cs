namespace SmartCookbook.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> IsEmailInUse(string email);
}
