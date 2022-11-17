using AutoMapper;
using SmartCookbook.Application.Services.CurrentUser;
using SmartCookbook.Comunicacao.Request;
using SmartCookbook.Comunicacao.Response;
using SmartCookbook.Domain.Repositories;
using SmartCookbook.Domain.Repositories.Recipe;
using SmartCookbook.Exceptions.ExceptionsBase;

namespace SmartCookbook.Application.UseCases.Recipe.Save;

public class SaveRecipeUseCase : ISaveRecipeUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnityOfWork _unityOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IRecipeWriteOnlyRepository _repository;

    public SaveRecipeUseCase(IMapper mapper, IUnityOfWork unityOfWork, ICurrentUser currentUser, IRecipeWriteOnlyRepository repository)
    {
        _mapper = mapper;
        _unityOfWork = unityOfWork;
        _currentUser = currentUser;
        _repository = repository;
    }

    public async Task<RecipeResponseJson> Execute(RecipeRequestJson request)
    {
        Validate(request);

        var currentUser = await _currentUser.GetCurrentUser();

        var recipe = _mapper.Map<Domain.Entities.Recipe>(request);
        recipe.UserId = currentUser.Id;

        await _repository.Save(recipe);

        await _unityOfWork.Commit();

        return _mapper.Map<RecipeResponseJson>(recipe);
    }

    private static void Validate(RecipeRequestJson request)
    {
        var validator = new SaveRecipeValidator();
        var result = validator.Validate(request);

        if(!result.IsValid)
        {
            var errorMessages = result.Errors.ConvertAll(e => e.ErrorMessage);
            throw new ValidationErrorException(errorMessages);
        }
    }
}