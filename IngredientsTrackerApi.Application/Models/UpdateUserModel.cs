using IngredientsTrackerApi.Application.Models.Dto;
using IngredientsTrackerApi.Application.Models.Identity;

namespace IngredientsTrackerApi.Application.Models;

public class UpdateUserModel
{
    public TokensModel Tokens { get; set; }

    public UserDto User { get; set; }
}
