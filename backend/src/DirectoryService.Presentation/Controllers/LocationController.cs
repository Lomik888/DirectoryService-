using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations.Command;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.Error;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

public class LocationController : ApplicationController
{
    [HttpPost("/locations")]
    public async Task<ActionResult<Guid>> CreateAsync(
        [FromBody] CreateLocationRequest request,
        [FromServices] ICommandHandler<Guid, List<Error>, CreateLocationCommand> handler)
    {
        var command = new CreateLocationCommand(request);
        var requestResult = await handler.HandleAsync(command, CancellationToken.None);

        if (requestResult.IsFailure == true)
            return BadRequest(requestResult.Error);

        return Ok(requestResult.Value);
    }
}