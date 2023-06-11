using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using RPGAPI.Dtos.Requests;
using RPGAPI.Dtos.Responses;
using RPGAPI.Infrastructure;
using RPGAPI.Services.Interfaces;
using RPGAPI.Utils;

namespace RPGAPI.Application.User.Commands
{
    public class CreateUserCommandHandler
    {
        public class Command : CreateUserDto, IRequest<UserDto>
        {

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(
                IStringLocalizer<CommonResources> commonLocalizer
            )
            {
                //Required
                RuleFor(x => x.UserName).NotNull().NotEmpty()
                   .WithMessage(commonLocalizer["COMMON_ERROR_INPUT_REQUIRED", nameof(Command.UserName), "50"].Value);

                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress()
                   .WithMessage(commonLocalizer["COMMON_ERROR_INPUT_REQUIRED", nameof(Command.Email)].Value);

                RuleFor(x => x.Password).NotNull().NotEmpty()
                   .WithMessage(commonLocalizer["COMMON_ERROR_INPUT_REQUIRED", nameof(Command.Password), "50"].Value);


               // RuleFor(x => x.Visibility)
               //.Must(x => EnumValidator.TryParseWithMemberName<SpaceVisibilityEnum>(x.ToString(), out _))
               //   .WithMessage(commonLocalizer["COMMON_ERROR_INVALID_INPUT_ENUM", nameof(Command.Visibility)].Value)
               //   .When(x => !string.IsNullOrEmpty(x.Visibility.ToString()));

               // RuleFor(x => x.Privacy)
               //.Must(x => EnumValidator.TryParseWithMemberName<SpacePrivacyEnum>(x.ToString(), out _))
               //   .WithMessage(commonLocalizer["COMMON_ERROR_INVALID_INPUT_ENUM", nameof(Command.Privacy)].Value)
               //   .When(x => !string.IsNullOrEmpty(x.Privacy.ToString()));
            }
        }

        public class Handler : IRequestHandler<Command, UserDto>
        {
            private readonly ServiceDbContext _context;
            private readonly IMapper _mapper;
            private readonly CommonValidator _commonValidator;
            private readonly IUserService _userService;
            private readonly IStringLocalizer<CommonResources> _commonLocalizer;

            public Handler(
                ServiceDbContext context,
                IMapper mapper,
                CommonValidator commonValidator,
                IUserService userService,
                IStringLocalizer<CommonResources> commonLocalizer
            )
            {
                _context = context;
                _mapper = mapper;
                _commonValidator = commonValidator;
                _commonLocalizer = commonLocalizer;
                _userService = userService;
            }

            public async Task<UserDto> Handle(
                            Command command,
                            CancellationToken cancellationToken
                        )
            {
                _commonValidator.Validate<Command>(command, new CommandValidator(_commonLocalizer));
                return _userService.AddUser(command);
            }
        }
    }
}
