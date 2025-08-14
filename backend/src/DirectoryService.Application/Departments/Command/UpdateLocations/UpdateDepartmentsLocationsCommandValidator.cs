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
    }
}