using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG.BuildingBlocks.Common.Extensions;
using RPG.Character.Application.Queries;
using RPG.Character.Domain.CharacterAggregate;
using CharacterEntity = RPG.Character.Domain.CharacterAggregate.Character;

namespace RPG.Character.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CharacterController : ControllerBase
    {

        private readonly ILogger<CharacterController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public CharacterController(
            ILogger<CharacterController> logger, 
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{characteId}")]
        public CharacterEntity GetCharacterById([FromRoute] Guid characterId)
        {

            return null;
        }

        [HttpGet]
        public async Task<IEnumerable<CharacterEntity>> GetCharacters()
        {
            GetCharactersByUserQueryHandler.Query query = new GetCharactersByUserQueryHandler.Query();
            query.UserId = Guid.Parse(await _httpContextAccessor.HttpContext.GetUserId());
            return await _mediator.Send(query);
        }
    }
}