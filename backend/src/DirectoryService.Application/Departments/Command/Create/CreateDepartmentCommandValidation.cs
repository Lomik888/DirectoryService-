using DirectoryService.Application.Extations;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.LocationValueObjects;
using FluentValidation;

namespace DirectoryService.Application.Departments.Command;

public class CreateDepartmentCommandValidation : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidation()
    {
        RuleFor(x => x.Request.Name).MustBeValueObject(x => DepartmentName.Create(x));
        RuleFor(x => x.Request.Identifier).MustBeValueObject(x => Identifier.Create(x));
        When(x => x.Request.ParentId != null, () =>
        {
            RuleFor(x => x.Request.ParentId).MustBeValueObject(x => DepartmentId.Create((Guid)x!));
        });

        RuleFor(x => x.Request.LocationIds)
            .Must(x => x.Count > 0 == true)
            .WithMessage(GeneralErrors.Validation.InvalidField(
                "Коллекция LocationIds не может быть пустой",
                "invalid.request.field"))
            .DependentRules( () =>
            {
                RuleFor(x => x.Request.LocationIds).Must(x => x.Count == x.Distinct().Count())
                    .WithMessage(GeneralErrors.Validation.InvalidField(
                        "Коллекция LocationIds имеет дубликаты",
                        "invalid.request.field"))
                    .DependentRules(() =>
                    {
                        RuleForEach(x => x.Request.LocationIds)
                            .MustBeValueObject(x => LocationId.Create((Guid)x!));
                    });
            });
    }
}