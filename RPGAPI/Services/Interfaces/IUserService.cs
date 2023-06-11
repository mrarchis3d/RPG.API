using RPGAPI.Controllers;
using RPGAPI.Dtos.Requests;
using RPGAPI.Dtos.Responses;

namespace RPGAPI.Services.Interfaces
{
    public interface IUserService
    {
        UserDto AddUser(CreateUserDto user);
        bool ValidateUserCredentials(UserCredentialsDto credentials);
        string GenerateAccessToken(string username);
    }
}
