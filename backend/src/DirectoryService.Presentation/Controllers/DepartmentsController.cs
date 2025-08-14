using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Departments.Command;
using DirectoryService.Application.Departments.Command.Transfer;
using DirectoryService.Application.Departments.Command.UpdateLocations;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.Err;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

public class DepartmentsController : ApplicationController
{
    [HttpPost("/departments")]
    public async Task<CustomResult<Guid>> CreateAsync(
        [FromBody] CreateDepartmentRequest request,
        [FromServices] ICommandHandler<Guid, Errors, CreateDepartmentCommand> handler,
        CancellationToken cancellationToken)
    {
        return (await handler.HandleAsync(request, cancellationToken), StatusCodes.Status201Created);
    }

    [HttpPut("/departments/{departmentId::guid}/locations")]
    public async Task<CustomResult<Guid>> UpdateAsync(
        [FromRoute] Guid departmentId,
        [FromBody] UpdateDepartmentsLocationsRequest request,
        [FromServices] ICommandHandler<Guid, Errors, UpdateDepartmentsLocationsCommand> handler,
        CancellationToken cancellationToken)
    {
        return (await handler.HandleAsync(
                new UpdateDepartmentsLocationsCommand(departmentId, request),
                cancellationToken),
            StatusCodes.Status200OK);
    }


    [HttpPut("/departments/{departmentId::guid}/parent")]
    public async Task<CustomResult<Guid>> TransferAsync(
        [FromRoute] Guid departmentId,
        [FromBody] TransferDepartmentRequest request,
        [FromServices] ICommandHandler<Errors, TransferDepartmentCommand> handler,
        CancellationToken cancellationToken)
    {
        return await handler.HandleAsync(
            new TransferDepartmentCommand(departmentId, request.ParentId),
            cancellationToken);
    }
}