using FluentValidation;
using FluentValidation.Results;
using RPG.BuildingBlocks.Common.Exceptions;
using System.Net;

namespace RPG.BuildingBlocks.Common.Utils
{
    public static class CommonValidator
    {
        public static ValidationResult Validate<TCommand>(TCommand command, AbstractValidator<TCommand> validator)
        {
            var results = validator.Validate(command);
            if (!results.IsValid)
            {
                if (results.Errors.Any(x => x.ErrorCode.Equals(Constants.EventBus.UNRECOVERABLE_ERROR)))
                    throw new AppException(results.Errors[0].ErrorMessage, HttpStatusCode.NotFound, true);

                throw new AppException(results.Errors[0].ErrorMessage);
            }

            return results;
        }
    }
}
