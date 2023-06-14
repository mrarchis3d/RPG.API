using FluentValidation;
using FluentValidation.Results;

namespace RPG.Identity.Utils
{
    public class CommonValidator
    {
        public ValidationResult Validate<TCommand>(TCommand command, AbstractValidator<TCommand> validator)
        {
            var results = validator.Validate(command);
            if (!results.IsValid)
            {
                throw new AppException(results.Errors[0].ErrorMessage);
            }

            return results;
        }
    }
}
