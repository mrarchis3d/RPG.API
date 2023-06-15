using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace RPG.Identity.Utils
{
    public class MultilanguageIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<ServiceResources> _serviceLocalizer;

        public MultilanguageIdentityErrorDescriber(IStringLocalizer<ServiceResources> serviceLocalizer)
        {
            _serviceLocalizer = serviceLocalizer;
        }

        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = _serviceLocalizer["ERROR_INCORRECT_PASSWORD"].Value }; }
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = _serviceLocalizer["ERROR_PASSWORD_LENGTH", length].Value }; }
    }
}
