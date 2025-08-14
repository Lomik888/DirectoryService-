using DirectoryService.Application.Extations;
using DirectoryService.Domain.DepartmentValueObjects;
using FluentValidation;

namespace DirectoryService.Application.Departments.Command.Transfer;

public class TransferDepartmentCommandValidator : AbstractValidator<TransferDepartmentCommand>
{
    public TransferDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId).MustBeValueObject(x => DepartmentId.Create(x));
        When(x => x.ParentId != null, () =>
        {
            RuleFor(x => x.ParentId).MustBeValueObject(x => DepartmentId.Create((Guid)x!));
        });
    }
}