using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Extations;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.DepartmentValueObjects;
using DirectoryService.Domain.Err;
using DirectoryService.Domain.PositionValueObjects;
using FluentValidation;
using FluentValidation.Results;

namespace DirectoryService.Application.Positions.Command;

public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
{
    public CreatePositionCommandValidator(
        IPositionRepository positionRepository,
        IDepartmentsRepository departmentsRepository)
    {
        RuleFor(x => x.Request.Name).MustBeValueObject(x => PositionName.Create(x));
        RuleFor(x => x.Request.Description).MustBeValueObject(x => Description.Create(x));

        RuleFor(x => x.Request.DepartmentIds).NotNull().NotEmpty()
            .WithMessage(GeneralErrors.Validation.InvalidField(
                "Коллекция DepartmentIds не может быть пустой",
                "invalid.request.field"));

        RuleFor(x => x.Request.DepartmentIds).Must(x => x.Count == x.Distinct().Count())
            .WithMessage(GeneralErrors.Validation.InvalidField(
                "Коллекция DepartmentIds имеет дубликаты",
                "invalid.request.field"));

        RuleForEach(x => x.Request.DepartmentIds)
            .MustBeValueObject(x => DepartmentId.Create(x))
            .DependentRules(() =>
            {
                RuleFor(x => x.Request.Name)
                    .MustAsync(async (name, cancellationToken) =>
                    {
                        var positionName = PositionName.Create(name).Value;

                        var exists = await positionRepository.NameIsUniqueAsync(positionName, cancellationToken);
                        return exists;
                    })
                    .WithMessage(GeneralErrors.Validation.InvalidField(
                        "Активная позиция с таким именем уже существует",
                        "position.name.exists"))
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.Request.DepartmentIds)
                            .MustCustomAsync(async (ids, cancellationToken) =>
                            {
                                var idsVo = ids.Select(x => DepartmentId.Create(x).Value).ToList();

                                var exists = await departmentsRepository.DepartmentsIsActiveAndExistsAsync(
                                    idsVo,
                                    cancellationToken);
                                if (exists.Count != idsVo.Count)
                                {
                                    var invalidIds = idsVo.Except<DepartmentId>(exists).ToList();

                                    var idsMessage = string.Join(", ", invalidIds.Select(x => x.Value));
                                    var error = Error.Create(
                                        $"Ids {idsMessage} не являются активными или не сущессвуют",
                                        "invalid.departments.ids", ErrorTypes.VALIDATION);
                                    return error;
                                }

                                return UnitResult.Success<Error>();
                            });
                    });
            });
    }
}