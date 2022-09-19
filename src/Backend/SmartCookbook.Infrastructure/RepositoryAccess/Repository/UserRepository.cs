using Microsoft.EntityFrameworkCore;
using SmartCookbook.Domain.Entities;
using SmartCookbook.Domain.Repositories;

namespace SmartCookbook.Infrastructure.RepositoryAccess;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
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
}
