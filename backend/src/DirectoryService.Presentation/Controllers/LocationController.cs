using System.Net;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations.Command;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.Error;
using DirectoryService.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

public class LocationController : ApplicationController
{
    /// <summary>
    /// Создание Локации
    /// </summary>
    /// <returns>Location's Guid.</returns>
    [HttpPost("/locations")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(Envelope<Errors>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(Envelope<Errors>), (int)HttpStatusCode.InternalServerError)]
    public async Task<CustomResult<Guid>> CreateAsync(
        [FromBody] CreateLocationRequest request,
        [FromServices] ICommandHandler<Guid, Errors, CreateLocationCommand> handler,
        CancellationToken cancellationToken)
    {
        return (await handler.HandleAsync(request, cancellationToken), StatusCodes.Status201Created);
    }
}