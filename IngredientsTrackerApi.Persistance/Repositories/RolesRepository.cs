using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Domain.Entities.Identity;
using IngredientsTrackerApi.Persistance.Database;

namespace IngredientsTrackerApi.Persistance.Repositories;

public class RolesRepository : BaseRepository<Role>, IRolesRepository
{
    public RolesRepository(MongoDbContext db) : base(db, "Roles") { }
}