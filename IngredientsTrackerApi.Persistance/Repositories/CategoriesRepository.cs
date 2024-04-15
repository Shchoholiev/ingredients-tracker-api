using IngredientsTrackerApi.Application.IRepositories;
using IngredientsTrackerApi.Domain.Entities;
using IngredientsTrackerApi.Persistance.Database;

namespace IngredientsTrackerApi.Persistance.Repositories;

public class CategoriesRepository(MongoDbContext db) 
    : BaseRepository<Category>(db, "Categories"), ICategoriesRepository
{
}
