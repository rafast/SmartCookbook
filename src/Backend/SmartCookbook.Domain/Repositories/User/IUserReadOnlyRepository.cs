namespace SmartCookbook.Domain.Repositories;

public interface IUserReadOnlyRepository
{
    Task<bool> IsEmailInUse(string email);
    Task<Entities.User> RecuperaPorEmailSenha(string email, string password);
    Task<Entities.User> GetByEmail(string email);
}
