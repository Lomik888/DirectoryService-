namespace DirectoryService.Presentation.Responses;

public record ErrorResponse(string ErrorMessage, string ErrorCode, string ErrorType);