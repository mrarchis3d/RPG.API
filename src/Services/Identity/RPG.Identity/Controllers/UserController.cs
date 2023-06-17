using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPG.Identity.Application.User.Commands;
using RPG.Identity.Dtos.Responses;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RPG.Identity.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController: Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("createuserbyemail")]
        [SwaggerOperation(Summary = "create user by email")]
        [SwaggerResponse((int)HttpStatusCode.OK, "user was created")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "user does not created")]
        public async Task<UserStandardResponse> CreateUserByEmail(
            [FromBody] CreateUserCommandHandler.Command command
        )
        {
            return await _mediator.Send(command);
        }
    }
}
