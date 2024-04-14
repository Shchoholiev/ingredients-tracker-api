using System.Security.Claims;
using MongoDB.Bson;
using IngredientsTrackerApi.Application.Models.GlobalInstances;

namespace IngredientsTrackerApi.RestApi.Middlewares;

public class GlobalUserCustomMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (ObjectId.TryParse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out ObjectId id))
        {
            GlobalUser.Id = id;
        }
        GlobalUser.Name = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        GlobalUser.Email = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        GlobalUser.Phone = httpContext.User.FindFirst(ClaimTypes.MobilePhone)?.Value;
        
        GlobalUser.Roles = new List<string>();
        foreach (var role in httpContext.User.FindAll(ClaimTypes.Role))
        {
            GlobalUser.Roles.Add(role.Value);
        }

        await this._next(httpContext);
    }
}
