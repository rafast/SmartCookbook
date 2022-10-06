using Microsoft.EntityFrameworkCore;
using SmartCookbook.Domain.Entities;
using SmartCookbook.Domain.Repositories;
using SmartCookbook.Domain.Repositories.User;

namespace SmartCookbook.Infrastructure.RepositoryAccess;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly SmartCookbookContext _context;

    public UserRepository(SmartCookbookContext context)
    {
        _context = context;
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<bool> IsEmailInUse(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User> RecuperaPorEmailSenha(string email, string password)
    {
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Password.Equals(password));
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }
}
