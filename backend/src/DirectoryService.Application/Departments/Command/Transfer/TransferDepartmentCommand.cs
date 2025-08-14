using DirectoryService.Application.Abstractions;

namespace DirectoryService.Application.Departments.Command.Transfer;

public record TransferDepartmentCommand(Guid DepartmentId, Guid? ParentId) : ICommand;