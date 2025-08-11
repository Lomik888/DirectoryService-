using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Positions.Command;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.Err;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

public class PositionController : ApplicationController
{
    [HttpPost("/positions")]
    public async Task<CustomResult<Guid>> CreateAsync(
        [FromBody] CreatePositionRequest request,
        [FromServices] ICommandHandler<Guid, Errors, CreatePositionCommand> handler,
        CancellationToken cancellationToken)
    {
        return (await handler.HandleAsync(request, cancellationToken), StatusCodes.Status201Created);
    }
}