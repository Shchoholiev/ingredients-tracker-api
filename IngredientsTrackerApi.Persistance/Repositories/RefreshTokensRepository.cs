using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Domain.Entities.Identity;
using IngredientsTrackerApi.Persistance.Database;

namespace IngredientsTrackerApi.Persistance.Repositories;

public class RefreshTokensRepository : BaseRepository<RefreshToken>, IRefreshTokensRepository
{
    public RefreshTokensRepository(MongoDbContext db) : base(db, "RefreshTokens") { }
}
