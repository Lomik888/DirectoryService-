using CSharpFunctionalExtensions;
using DirectoryService.Application.Extations;
using DirectoryService.Application.Locations;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.LocationValueObjects;
using FluentValidation;

namespace DirectoryService.Application.Departments.Command.UpdateLocations;

public class UpdateDepartmentsLocationsCommandValidator : AbstractValidator<UpdateDepartmentsLocationsCommand>
{
    public const string DB_VALIDATION = nameof(DB_VALIDATION);

    public UpdateDepartmentsLocationsCommandValidator(
        IDepartmentsRepository departmentsRepository,
        ILocationRepository locationRepository)
    {
        RuleFor(x => x.DepartmentId).MustBeValueObject(x => DepartmentId.Create(x));

        RuleFor(x => x.Request.LocationIds)
            .Must(x => x.Count > 0 == true && x.Distinct().Count() == x.Count)
            .WithMessage(GeneralErrors.Validation.InvalidField(
                "Коллекция LocationIds не может быть пустой или с дубликатами",
                "invalid.request.field"));

        RuleSet("DB_VALIDATION", () =>
        {
            RuleFor(x => x.Request.LocationIds)
                .MustCustomAsync(async (x, cancellationToken) =>
                {
                    var ids = x.Select(x => LocationId.Create(x).Value).ToList();

                    var validIds =
                        await locationRepository.LocationsIsActiveAndExistsAsync(ids, cancellationToken);
                    if (ids.Count != validIds.Count)
                    {
                        var invalidIds = ids.Except(validIds);

                        var idsMessage = string.Join(", ", invalidIds.Select(x => x.Value));
                        var error = Error.Create(
                            $"Ids локаций {idsMessage} не являются активными или не сущессвуют",
                            "invalid.locations.ids", ErrorTypes.VALIDATION);
                        return error;
                    }

                    return UnitResult.Success<Error>();
                });
        });
    }
}