using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Dtos;

namespace DirectoryService.Application.Locations.Command;

public sealed record CreateLocationCommand(
    string LocationName,
    AdressDto AddressDto,
    string TimeZone) : ICommand;