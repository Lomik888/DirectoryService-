using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Requests;

namespace DirectoryService.Application.Positions.Command;

public record CreatePositionCommand(CreatePositionRequest Request) : ICommand
{
    public static implicit operator CreatePositionCommand(CreatePositionRequest request)
    {
        return new CreatePositionCommand(request);
    }
}