namespace DirectoryService.Contracts.Requests;

public record CreateLocationRequest(
    string LocationName,
    string City,
    string Street,
    string HouseNumber,
    string? Number,
    string TimeZone);