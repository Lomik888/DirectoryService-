namespace DirectoryService.Application.Dtos;

public sealed record AdressDto(
    string City,
    string Street,
    string HouseNumber,
    string? Number);