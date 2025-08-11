using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Positions;
using DirectoryService.Infrastructure.Options;
using DirectoryService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var psqlSection = configuration.GetRequiredSection(PsqlOptions.SECTION);
        var connectionString = psqlSection.GetValue<string>(PsqlOptions.CONNECTION_STRING) ??
                               throw new NullReferenceException("Postgres SQL Connection String missing");

        services.AddScoped<ITransactionManager, TransactionManager<DirectoryServiceDbContext>>();

        services.AddDbContext<DirectoryServiceDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}