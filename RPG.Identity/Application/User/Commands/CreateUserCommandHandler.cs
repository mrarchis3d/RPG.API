using AutoMapper;
using FluentValidation;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using RPG.BuildingBlocks.Common.Utils;
using RPG.BuildingBlocks.Utils;
using RPG.Identity.Domain.UserAggregate;
using RPG.Identity.Dtos.Requests;
using RPG.Identity.Dtos.Responses;
using RPG.Identity.Utils;
using System.Security.Claims;

namespace RPG.Identity.Application.User.Commands
{
    public class CreateUserCommandHandler
    {
        public class Command : CreateUserDto, IRequest<UserStandardResponse>
        {

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(
                IStringLocalizer<ServiceResources> commonLocalizer
            )
            {
                //Required
                RuleFor(x => x.UserName).NotNull().NotEmpty()
                   .WithMessage(commonLocalizer["COMMON_ERROR_INPUT_REQUIRED", nameof(Command.UserName), "50"].Value);

                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress()
                   .WithMessage(commonLocalizer["COMMON_ERROR_INPUT_REQUIRED", nameof(Command.Email)].Value);

                RuleFor(x => x.Password).NotNull().NotEmpty()
                   .WithMessage(commonLocalizer["COMMON_ERROR_INPUT_REQUIRED", nameof(Command.Password), "50"].Value);

            }
        }

        public class Handler : IRequestHandler<Command, UserStandardResponse>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;
            private readonly CommonValidator _commonValidator;
            private readonly IStringLocalizer<ServiceResources> _serviceLocalizer;

            public Handler(
                UserManager<ApplicationUser> userManager,
                IMapper mapper,
                CommonValidator commonValidator,
                IStringLocalizer<ServiceResources> serviceResources,
                ILogger<Handler> logger
            )
            {
                _userManager  = userManager;
                _mapper = mapper;
                _commonValidator = commonValidator;
                _serviceLocalizer = serviceResources;
                _logger = logger;
            }

            public async Task<UserStandardResponse> Handle(
                            Command command,
                            CancellationToken cancellationToken
                        )
            {
                _commonValidator.Validate<Command>(command, new CommandValidator(_serviceLocalizer));
                _commonValidator.Validate<string>(command.Password, new PasswordValidator(_serviceLocalizer));

                var user = await _userManager.FindByEmailAsync(command.Email);

                if (user != null && user.EmailConfirmed)
                {
                    throw new AppException(_serviceLocalizer["ERROR_EMAIL_EXIST"].Value);
                }

                user = new ApplicationUser
                {
                    UserName = command.UserName,
                    Email = command.Email,
                    Name = command.FullName,
                    EmailConfirmed = false
                };



                var result = await _userManager.CreateAsync(user);

                user = await _userManager.FindByNameAsync(user.UserName);
                await _userManager.AddClaimsAsync(user, new Claim[]
                {
                    new Claim(JwtClaimTypes.Email, command.Email),
                    new Claim("UserName", user.UserName)
                });

                var passwordResult = await _userManager.AddPasswordAsync(user, command.Password);
                if (!passwordResult.Succeeded)
                {
                    _logger.LogCritical(passwordResult.Errors);
                    throw new AppException(result.Errors.ElementAt(0).Description);
                }

                return _mapper.Map<UserStandardResponse>(user);
            }

            private async Task<string> CreateTemporalUserName()
            {
                var newUserName = String.Empty;
                var newUser = new ApplicationUser();
                do
                {
                    newUserName = $"RPG_{Guid.NewGuid()}";
                    newUser = await _userManager.FindByNameAsync(newUserName);
                }
                while (newUser != null);

                return newUserName;
            }
        }

    }
}
