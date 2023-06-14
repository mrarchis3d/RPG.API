using RPG.Identity.Controllers;
using RPG.Identity.Dtos.Requests;
using RPG.Identity.Dtos.Responses;

namespace RPG.Identity.Services.Interfaces
{
    public interface IUserService
    {
        UserDto AddUser(CreateUserDto user);
        bool ValidateUserCredentials(UserCredentialsDto credentials);
        string GenerateAccessToken(string username);
    }
}
