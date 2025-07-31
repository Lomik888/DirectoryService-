using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations.Command;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.Error;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

public class LocationController : ApplicationController
{
    [HttpPost("/locations")]
    public async Task<CustomResult<Guid>> CreateAsync(
        [FromBody] CreateLocationRequest request,
        [FromServices] ICommandHandler<Guid, Errors, CreateLocationCommand> handler,
        CancellationToken cancellationToken)
    {
        return (await handler.HandleAsync(request, cancellationToken), StatusCodes.Status201Created);
    }
}