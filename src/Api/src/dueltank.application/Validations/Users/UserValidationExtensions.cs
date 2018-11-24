using FluentValidation;

namespace dueltank.application.Validations.Users
{
    public static class UserValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> UserIdValidator<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotNull()
                .SetValidator(new UserIdValidator());
        }
    }
}