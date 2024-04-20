using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using IngredientsTrackerApi.Application.IServices;
using IngredientsTrackerApi.Application.IServices.Identity;
using IngredientsTrackerApi.Infrastructure.Services.Identity;
using IngredientsTrackerApi.Infrastructure.Services;

namespace IngredientsTrackerApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRolesService, RolesService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<ITokensService, TokensService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IGroupsService, GroupsService>();
        services.AddScoped<IDevicesService, DevicesService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<ICloudStorageService, CloudStorageService>();
        services.AddScoped<IImagesService, ImagesService>();
        services.AddScoped<IRecipesService, RecipesService>();
        services.AddScoped<IImageRecognitionService, ImageRecognitionService>();
        services.AddScoped<IProductsRecognizerService, ProductsRecognizerService>();
        
        services.AddScoped<IComputerVisionClient, ComputerVisionClient>(
            client => new ComputerVisionClient(new ApiKeyServiceClientCredentials(configuration.GetValue<string>("AzureCognitiveServices:ComputerVision:Key")))
            {
                Endpoint = configuration.GetValue<string>("AzureCognitiveServices:ComputerVision:Endpoint")
            }
        );

        return services;
    }

    public static IServiceCollection AddJWTTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateIssuer"),
                    ValidateAudience = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateAudience"),
                    ValidateLifetime = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateLifetime"),
                    ValidateIssuerSigningKey = configuration.GetValue<bool>("JsonWebTokenKeys:ValidateIssuerSigningKey"),
                    ValidIssuer = configuration.GetValue<string>("JsonWebTokenKeys:ValidIssuer"),
                    ValidAudience = configuration.GetValue<string>("JsonWebTokenKeys:ValidAudience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JsonWebTokenKeys:IssuerSigningKey"))),
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddAuthorization();

        return services;
    }
}
