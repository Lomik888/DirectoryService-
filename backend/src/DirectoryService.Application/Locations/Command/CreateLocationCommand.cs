using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Dtos;
using DirectoryService.Contracts.Requests;

namespace DirectoryService.Application.Locations.Command;

public sealed record CreateLocationCommand(CreateLocationRequest Request) : ICommand
{
    public static implicit operator CreateLocationCommand(CreateLocationRequest request) => new(request);
};