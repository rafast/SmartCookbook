using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using SmartCookbook.Application.Services.Token;
using SmartCookbook.Comunicacao.Response;
using SmartCookbook.Domain.Repositories;
using SmartCookbook.Exceptions;
using System.Threading.Tasks;

namespace SmartCookbook.API.Filters;

public class AuthenticatedUser : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    private readonly TokenController _tokenController;
    private readonly IUserReadOnlyRepository _repository;

    public AuthenticatedUser(TokenController tokenController, IUserReadOnlyRepository repository)
    {
        _tokenController = tokenController;
        _repository = repository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);
            var userEmail = _tokenController.GetEmail(token);

            var currentUser = await _repository.GetByEmail(userEmail);

            if (currentUser is null)
            {
                throw new System.Exception();
            }
        }
        catch (SecurityTokenExpiredException)
        {
            ExpiredToken(context);
        }
        catch
        {
            UserNotAllowed(context);
        }

    }

    private string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authorization))
        {
            throw new System.Exception();
        }

        return authorization["Bearer".Length..].Trim();
    }

    private void ExpiredToken(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseJson(ResourceErrorMessages.EXPIRED_TOKEN));
    }

    private void UserNotAllowed(AuthorizationFilterContext context)
    {
        context.Result = new UnauthorizedObjectResult(new ErrorResponseJson(ResourceErrorMessages.NOT_ALLOWED_USER));
    }
}
