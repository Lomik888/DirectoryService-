using CSharpFunctionalExtensions;
using DirectoryService.Domain.Error;
using FluentValidation;
using FluentValidation.Results;

namespace DirectoryService.Application.Extations;

public static class FluentValidationExtations
{
    public static IRuleBuilderOptionsConditions<T, TProperty> MustBeValueObject<T, TProperty, TVo>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        Func<TProperty, Result<TVo, IEnumerable<Error>>> predicate)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = predicate(value);

            if (result.IsSuccess != false)
            {
                return;
            }

            foreach (var error in result.Error)
            {
                context.AddFailure(error.Message);
            }
        });
    }

    public static List<Error> ToErrors(this List<ValidationFailure> validationFailures)
    {
        var errors = validationFailures.Select(x => Error.Create(x.ErrorMessage));
        return errors.ToList();
    }
}