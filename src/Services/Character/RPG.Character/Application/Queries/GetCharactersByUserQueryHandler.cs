using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RPG.BuildingBlocks.Common.Utils;
using RPG.Character.Infrastructure;
using CharacterEntity = RPG.Character.Domain.CharacterAggregate.Character;

namespace RPG.Character.Application.Queries
{
    public class GetCharactersByUserQueryHandler
    {
        public class Query : IRequest<IEnumerable<CharacterEntity>>
        {
            public Guid UserId { get; set; }

        }

        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator(
                IStringLocalizer commonLocalizer
            )
            {
                //Required
                RuleFor(x => x.UserId).NotNull().NotEmpty()
                   .WithMessage(commonLocalizer["COMMON_ERROR_INPUT_REQUIRED", nameof(Query.UserId)].Value);

            }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<CharacterEntity>>
        {
            private readonly IMapper _mapper;
            private readonly ServiceDbContext _context;
            private readonly ILogger<Handler> _logger;
            private readonly IStringLocalizer _commonLocalizer;

            public Handler(
                IMapper mapper,
                IStringLocalizer commonLocalizer,
                ILogger<Handler> logger,
                ServiceDbContext contex
            )
            {
                _mapper = mapper;
                _commonLocalizer = commonLocalizer;
                _logger = logger;
                _context = contex;
            }

            public async Task<IEnumerable<CharacterEntity>> Handle(
                            Query query,
                            CancellationToken cancellationToken
                        )
            {
                CommonValidator.Validate<Query>(query, new CommandValidator(_commonLocalizer));

                return await _context.Character
                    .Include(x=>x.ClassType)
                    .Include(x=>x.RaceType)
                    .Where(x => x.UserId == query.UserId).AsNoTracking().ToListAsync();
            }
        }
    }
}
