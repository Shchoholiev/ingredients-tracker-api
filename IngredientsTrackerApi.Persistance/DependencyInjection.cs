using Microsoft.Extensions.DependencyInjection;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Persistance.Database;
using IngredientsTrackerApi.Persistance.Repositories;

namespace IngredientsTrackerApi.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();
        
        services.AddScoped<IRolesRepository, RolesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();

        return services;
    }
}
