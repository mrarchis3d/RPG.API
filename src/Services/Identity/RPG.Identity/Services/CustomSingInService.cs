using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RPG.Identity.Infrastructure;
using RPG.Identity.Utils;

namespace RPG.Identity.Services
{
    public class CustomSignInService : ICustomTokenRequestValidator
    {
        private UserIdentityDbContext _context;
        private IStringLocalizer<ServiceResources> _serviceLocalizer;
        public CustomSignInService(UserIdentityDbContext context, IStringLocalizer<ServiceResources> serviceLocalizer)
        {
            _context = context;
            _serviceLocalizer = serviceLocalizer;
        }
        public async Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            var validatedRequest = context.Result.ValidatedRequest;

            if (validatedRequest.ClientId != BuildingBlocks.Common.Constants.Authorization.IDENTITY_CLIENT)
            {
                return;
            }

            var userId = validatedRequest.Subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if ( !context.Result.IsError)
            {
                context.Result.IsError = true;
                context.Result.Error = "not_active";
                return;
            }

            if (!(user.EmailConfirmed || user.PhoneNumberConfirmed))
            {
                context.Result.IsError = true;
                context.Result.Error = "not_confirmed";
                context.Result.CustomResponse = new Dictionary<string, object>();
                context.Result.CustomResponse.Add("error_description",
                    _serviceLocalizer["ERROR_USER_NOT_CONFIRMED", user.UserName].Value);
                context.Result.CustomResponse.Add("username", user.UserName);
                context.Result.CustomResponse.Add("has_phone", !string.IsNullOrEmpty(user.PhoneNumber));
                context.Result.CustomResponse.Add("has_email", !string.IsNullOrEmpty(user.Email));
                return;
            }
        }
    }
}
