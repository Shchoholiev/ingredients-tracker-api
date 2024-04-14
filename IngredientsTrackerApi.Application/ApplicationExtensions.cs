using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using IngredientsTrackerApi.Application.Mapping;

namespace IngredientsTrackerApi.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        // services.AddAutoMapper(Assembly.GetAssembly(typeof(UserProfile)));

        return services;
    }
}
