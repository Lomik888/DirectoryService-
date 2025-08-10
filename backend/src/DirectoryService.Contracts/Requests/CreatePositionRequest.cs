namespace DirectoryService.Contracts.Requests;

public record CreatePositionRequest(
    string Name,
    string? Description,
    List<Guid> DepartmentIds);