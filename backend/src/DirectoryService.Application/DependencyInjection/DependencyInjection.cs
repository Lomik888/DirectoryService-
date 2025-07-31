using DirectoryService.Application.Abstractions;
using DirectoryService.Domain.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddCommands();
        services.AddClock();
        services.AddValidations();
    }

    private static void AddValidations(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
    }

    private static void AddClock(this IServiceCollection services)
    {
        services.AddSingleton<IClock, Clock>();
    }

    private static void AddCommands(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblyDependencies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes =>
                classes.AssignableToAny(typeof(ICommandHandler<,,>), typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces());
    }
}