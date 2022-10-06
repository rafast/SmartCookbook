using SmartCookbook.Application.Cryptograph;
using SmartCookbook.Application.Services.CurrentUser;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Domain.Repositories;
using SmartCookbook.Domain.Repositories.User;
using SmartCookbook.Exceptions;
using SmartCookbook.Exceptions.ExceptionsBase;

namespace SmartCookbook.Application.UseCases.User.ChangePassword;
public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly PasswordCryptograph _cryptograph;
    private readonly IUnityOfWork _unityOfWork;

    public ChangePasswordUseCase(ICurrentUser currentUser, IUserUpdateOnlyRepository repository, PasswordCryptograph cryptograph, IUnityOfWork unityOfWork)
    {
        _currentUser = currentUser;
        _repository = repository;
        _cryptograph = cryptograph;
        _unityOfWork = unityOfWork;
    }

    public async Task Execute(ChangePasswordRequestJson request)
    {
        var currentUser = await _currentUser.GetCurrentUser();

        var user = await _repository.GetById(currentUser.Id);

        Validate(request, user);

        user.Password = _cryptograph.Cryptograph(request.NewPassword);

        _repository.Update(user);

        await _unityOfWork.Commit();
    }

    private void Validate(ChangePasswordRequestJson request, Domain.Entities.User user)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);
        var encryptedOldPassword = _cryptograph.Cryptograph(request.OldPassword);

        if (!user.Password.Equals(encryptedOldPassword))
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("oldPassword", ResourceErrorMessages.INVALID_OLDPASSWORD));
        }

        if (!result.IsValid)
        {
            var messages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ValidationErrorException(messages);
        }
    }
}
