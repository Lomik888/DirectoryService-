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
                var errorString = error.Serialize();
                context.AddFailure(errorString);
            }
        });
    }

    public static List<Error> ToErrors(this List<ValidationFailure> validationFailures)
    {
        var errors = validationFailures.Select(x => Error.Deserialize(x.ErrorMessage)).ToList();
        return errors;
    }

    public static IRuleBuilderOptionsConditions<T, TProperty> MustBeValueObject<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        Func<TProperty, List<Error>> predicate)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = predicate(value);

            if (result.Count == 0)
            {
                return;
            }

            foreach (var error in result)
            {
                var errorString = error.Serialize();
                context.AddFailure(errorString);
            }
        });
    }
}