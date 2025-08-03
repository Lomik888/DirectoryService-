using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations;
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
        services.AddScoped<ILocationRepository, LocationRepository>();
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