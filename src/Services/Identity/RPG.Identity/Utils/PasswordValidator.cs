using FluentValidation;
using Microsoft.Extensions.Localization;

namespace RPG.Identity.Utils
{
    public class PasswordValidator : AbstractValidator<string>
    {
        private static int MINIMUN_PASSWORD_LENGTH = 6;
        public PasswordValidator(IStringLocalizer serviceLocalizer)
        {
            RuleFor(x => x).NotNull().NotEmpty().MinimumLength(MINIMUN_PASSWORD_LENGTH)
                .WithMessage(serviceLocalizer["ERROR_PASSWORD_LENGTH", MINIMUN_PASSWORD_LENGTH].Value);
        }
    }
}
