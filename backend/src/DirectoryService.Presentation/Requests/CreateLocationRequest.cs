using DirectoryService.Application.Dtos;
using DirectoryService.Application.Locations.Command;
using DirectoryService.Presentation.Abstractions;

namespace DirectoryService.Presentation.Requests;

public record CreateLocationRequest(
    string LocationName,
    string City,
    string Street,
    string HouseNumber,
    string? Number,
    string TimeZone) : IToCommand<CreateLocationCommand>
{
    public CreateLocationCommand ToCommand()
    {
        return new CreateLocationCommand(
            LocationName,
            new AdressDto(City, Street, HouseNumber, Number),
            TimeZone);
    }
}