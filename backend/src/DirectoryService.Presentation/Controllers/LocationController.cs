using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations.Command;
using DirectoryService.Domain.Error;
using DirectoryService.Presentation.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

public class LocationController : ApplicationController
{
    [HttpPost("/locations")]
    public async Task<ActionResult<Guid>> CreateAsync(
        [FromBody] CreateLocationRequest request,
        [FromServices] ICommandHandler<Guid, List<Error>, CreateLocationCommand> handler)
    {
        var requestResult = await handler.HandleAsync(request.ToCommand(), CancellationToken.None);

        if (requestResult.IsFailure == true)
            return BadRequest(requestResult.Error);

        return Ok(requestResult.Value);
    }
}