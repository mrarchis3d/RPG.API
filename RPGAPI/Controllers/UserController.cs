using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPGAPI.Application.User.Commands;
using RPGAPI.Dtos.Responses;


namespace RPGAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;

    public UserController(
        IMediator mediator,
        ILogger<UserController> logger,
        IMapper mapper
        )
    {
        _mediator = mediator;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// create new user
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = null)]
    public async Task<UserDto> CreateUser([FromBody] CreateUserCommandHandler.Command command)
    {
        return await _mediator.Send(command);
    }


    //[HttpPost("authenticate")]
    //public IActionResult Authenticate([FromBody] UserCredentialsDto credentials)
    //{
    //    // Validar las credenciales del usuario (puedes implementar tu propia lógica de autenticación aquí)
    //    if (_userService.ValidateUserCredentials(credentials))
    //    {
    //        // Generar el token de acceso
    //        string accessToken = _userService.GenerateAccessToken(credentials.Username);

    //        // Devolver el token de acceso
    //        return Ok(new { access_token = accessToken });
    //    }

    //    // El usuario no está autenticado correctamente, devolver una respuesta de error
    //    return Unauthorized(new { error = "Credenciales inválidas" });
    //}
}


