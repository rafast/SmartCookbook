using SmartCookbook.Domain.Entities;

namespace SmartCookbook.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> IsEmailInUse(string email);
    Task<User> RecuperaPorEmailSenha(string email, string password);
}
