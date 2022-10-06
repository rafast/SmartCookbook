using Microsoft.AspNetCore.Http;
using SmartCookbook.Application.Services.Token;
using SmartCookbook.Domain.Entities;
using SmartCookbook.Domain.Repositories;

namespace SmartCookbook.Application.Services.CurrentUser;
public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContext;
    private readonly TokenController _tokenController;
    private readonly IUserReadOnlyRepository _repository;

    public CurrentUser(IHttpContextAccessor httpContext, TokenController tokenController, IUserReadOnlyRepository repository)
    {
        _httpContext = httpContext;
        _tokenController = tokenController;
        _repository = repository;
    }

    public async Task<User> GetCurrentUser()
    {
        var authorization = _httpContext.HttpContext.Request.Headers["Authorization"].ToString();

        var token = authorization["Bearer".Length..].Trim();

        var userEmail = _tokenController.GetEmail(token);

        var user = await _repository.GetByEmail(userEmail);

        return user;
    }
}
