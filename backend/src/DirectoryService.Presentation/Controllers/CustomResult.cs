using CSharpFunctionalExtensions;
using DirectoryService.Domain.Err;
using DirectoryService.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

public class CustomResult<T> : IActionResult
{
    private Envelope<T> Envelope { get; init; }

    private int? StatusCode { get; set; }

    private CustomResult(Envelope<T> envelope, int? statusCode = null)
    {
        Envelope = envelope;
        StatusCode = statusCode;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        if (Envelope?.Errors?.Count > 0)
        {
            StatusCode = Envelope.GetStatusCode();
        }

        var objectResult = new ObjectResult(Envelope) { StatusCode = StatusCode };

        await objectResult.ExecuteResultAsync(context);
    }

    public static implicit operator CustomResult<T>(Errors errors)
    {
        var envelope = Envelope<T>.Error(errors);
        var result = new CustomResult<T>(envelope);
        return result;
    }

    public static implicit operator CustomResult<T>((T value, int? statusCode) tuple)
    {
        var envelope = Envelope<T>.Ok(tuple.value);
        var customResult = new CustomResult<T>(envelope, tuple.statusCode);
        return customResult;
    }

    public static implicit operator CustomResult<T>((Result<T, Errors> result, int? statusCode) tuple)
    {
        Envelope<T> envelope;
        CustomResult<T> customResult;

        if (tuple.result.IsSuccess == true)
        {
            envelope = Envelope<T>.Ok(tuple.result.Value);
            customResult = new CustomResult<T>(envelope, tuple.statusCode ?? 200);
        }
        else
        {
            envelope = Envelope<T>.Error(tuple.result.Error);
            customResult = new CustomResult<T>(envelope);
        }

        return customResult;
    }
}