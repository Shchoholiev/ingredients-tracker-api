using Microsoft.Extensions.DependencyInjection;
using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Persistance.Database;

namespace IngredientsTrackerApi.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();
        
        return services;
    }
}
