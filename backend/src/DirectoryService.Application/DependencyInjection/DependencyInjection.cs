using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Factories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddCommands();
        services.AddClock();
        services.AddFactories();
        services.AddValidations();
    }

    private static void AddValidations(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
    }

    private static void AddFactories(this IServiceCollection services)
    {
        services.AddSingleton<LocationFactories>();
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