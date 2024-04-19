using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using IngredientsTrackerApi.Domain.Entities;
using IngredientsTrackerApi.Domain.Entities.Identity;
using IngredientsTrackerApi.Infrastructure.Services.Identity;
using IngredientsTrackerApi.Persistance.Database;
using IngredientsTrackerApi.Domain.Enums;

namespace IngredientsTrackerApi.IntegrationTests;

public class DbInitializer(MongoDbContext dbContext)
{
    private readonly MongoDbContext _dbContext = dbContext;

    public void InitializeDb()
    {
        _dbContext.Client.DropDatabase(_dbContext.Db.DatabaseNamespace.DatabaseName);
        
        InitializeUsersAsync().Wait();
        InitializeGroupsAsync().Wait();
        InitializeDevicesAsync().Wait();
        InitializeCategoriesAsync().Wait();
        InitializeProductsAsync().Wait();
        InitializeRecipesAsync().Wait();
    }

    public async Task InitializeUsersAsync()
    {
        #region Roles

        var rolesCollection = _dbContext.Db.GetCollection<Role>("Roles");

        var userRole = new Role
        {
            Name = "User"
        };
        await rolesCollection.InsertOneAsync(userRole);

        var ownerRole = new Role
        {
            Name = "Owner"
        };
        await rolesCollection.InsertOneAsync(ownerRole);

        var adminRole = new Role
        {
            Name = "Admin"
        };
        await rolesCollection.InsertOneAsync(adminRole);

        #endregion

        #region Users

        var passwordHasher = new PasswordHasher(new Logger<PasswordHasher>(new LoggerFactory()));
        var usersCollection = _dbContext.Db.GetCollection<User>("Users");

        var testUser = new User
        {
            Id = ObjectId.Parse("652c3b89ae02a3135d6409fc"),
            Email = "test@gmail.com",
            Phone = "+380123456789",
            Roles = new List<Role> { userRole },
            PasswordHash = passwordHasher.Hash("Yuiop12345"),
            CreatedById = ObjectId.Empty,
            CreatedDateUtc = DateTime.UtcNow
        };
        await usersCollection.InsertOneAsync(testUser);

        var updateTestUser = new User
        {
            Id = ObjectId.Parse("652c3b89ae02a3135d6309fc"),
            Email = "update@gmail.com",
            Phone = "+380123446789",
            Roles = new List<Role> { userRole },
            PasswordHash = passwordHasher.Hash("Yuiop12345"),
            CreatedById = ObjectId.Empty,
            CreatedDateUtc = DateTime.UtcNow
        };
        await usersCollection.InsertOneAsync(updateTestUser);

        var groupOwner = new User
        {
            Id = ObjectId.Parse("652c3b89ae02a3135d6419fc"),
            Email = "owner@gmail.com",
            Phone = "+380123456689",
            Roles = new List<Role> { userRole, ownerRole },
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // see group creation below
            PasswordHash = passwordHasher.Hash("Yuiop12345"),
            CreatedById = ObjectId.Empty,
            CreatedDateUtc = DateTime.UtcNow
        };
        await usersCollection.InsertOneAsync(groupOwner);

        var groupUser = new User
        {
            Id = ObjectId.Parse("652c3b89ae02a3135d6439fc"),
            Email = "group@gmail.com",
            Phone = "+380123456889",
            Roles = new List<Role> { userRole },
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // see group creation below
            PasswordHash = passwordHasher.Hash("Yuiop12345"),
            CreatedById = ObjectId.Empty,
            CreatedDateUtc = DateTime.UtcNow
        };
        await usersCollection.InsertOneAsync(groupUser);

        var groupUser2 = new User
        {
            Id = ObjectId.Parse("652c3b89ae02a3135d6432fc"),
            Email = "group2@gmail.com",
            Phone = "+380123456779",
            Roles = new List<Role> { userRole },
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // see group creation below
            PasswordHash = passwordHasher.Hash("Yuiop12345"),
            CreatedById = ObjectId.Empty,
            CreatedDateUtc = DateTime.UtcNow
        };
        await usersCollection.InsertOneAsync(groupUser2);

        var adminUser = new User
        {
            Id = ObjectId.Parse("652c3b89ae02a3135d6408fc"),
            Email = "admin@gmail.com",
            Phone = "+12345678901",
            Roles = new List<Role> { userRole, adminRole },
            PasswordHash = passwordHasher.Hash("Yuiop12345"),
            CreatedById = ObjectId.Empty,
            CreatedDateUtc = DateTime.UtcNow
        };
        await usersCollection.InsertOneAsync(adminUser);

        #endregion

        #region RefreshTokens

        var refreshTokensCollection = _dbContext.Db.GetCollection<RefreshToken>("RefreshTokens");

        var refreshToken = new RefreshToken
        {
            Token = "test-refresh-token",
            ExpiryDateUTC = DateTime.UtcNow.AddDays(-7),
            CreatedById = testUser.Id,
            CreatedDateUtc = DateTime.UtcNow
        };
        await refreshTokensCollection.InsertOneAsync(refreshToken);

        #endregion
    }

    public async Task InitializeGroupsAsync()
    {
        var groupsCollection = _dbContext.Db.GetCollection<Group>("Groups");

        var group = new Group
        {
            Id = ObjectId.Parse("652c3b89ae02a3135d6429fc"),
            Name = "Test Group 1",
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6419fc"),
            CreatedDateUtc = DateTime.UtcNow
        };
        await groupsCollection.InsertOneAsync(group);

        var secondGroup = new Group
        {
            Id = ObjectId.Parse("662c3b89ae02a3135d6429fc"),
            Name = "Test Group 2",
            CreatedById = ObjectId.Empty,
            CreatedDateUtc = DateTime.UtcNow
        };
        await groupsCollection.InsertOneAsync(secondGroup);
    }

    public async Task InitializeDevicesAsync()
    {
        var devicesCollection = _dbContext.Db.GetCollection<Device>("Devices");

        var device = new Device
        {
            Id = ObjectId.Parse("651c3b89ae02a3135d6439fc"),
            Name = "Test Device 1",
            Type = DeviceType.ProductsRecognizer,
            Guid = Guid.Parse("7a78a8b2-6cf6-427d-8ed2-a5e117d8fd3f"), 
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // See above
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await devicesCollection.InsertOneAsync(device);

        var updateDevice = new Device
        {
            Id = ObjectId.Parse("653c3b89ae02a3135d6439fc"),
            Name = "Test Device for Update",
            Type = DeviceType.ProductsRecognizer,
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await devicesCollection.InsertOneAsync(updateDevice);

        var accessPointevice = new Device
        {
            Id = ObjectId.Parse("753c3b89ae02a3135d6139fc"),
            Name = "Products Recognizer",
            Type = DeviceType.ProductsRecognizer,
            Guid = Guid.Parse("4d09b6ae-7675-4603-b632-9e834de6957f"), 
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await devicesCollection.InsertOneAsync(accessPointevice);
    }

    public async Task InitializeCategoriesAsync()
    {
        var categoriesCollection = _dbContext.Db.GetCollection<Category>("Categories");

        var category1 = new Category
        {
            Id = ObjectId.Parse("752c3b89ae02a3135d6429fc"),
            Name = "Breakfast",
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await categoriesCollection.InsertOneAsync(category1);

        var category2 = new Category
        {
            Id = ObjectId.Parse("762c3b89ae02a3135d6429fc"),
            Name = "Ukrainian",
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await categoriesCollection.InsertOneAsync(category2);
    }

    public async Task InitializeProductsAsync()
    {
        var productsCollection = _dbContext.Db.GetCollection<Product>("Products");

        var product1 = new Product
        {
            Id = ObjectId.Parse("801c3b89ae02a3135d6429fc"),
            Name = "Cheese",
            Count = 1,
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // See above
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await productsCollection.InsertOneAsync(product1);

        var product2 = new Product
        {
            Id = ObjectId.Parse("802c3b89ae02a3135d6429fc"),
            Name = "Chicken Breasts",
            Count = 5,
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // See above
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };

        await productsCollection.InsertOneAsync(product2);

        var product3 = new Product
        {
            Id = ObjectId.Parse("803c3b89ae02a3135d6429fc"),
            Name = "Beetroot",
            Count = 2,
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // See above
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await productsCollection.InsertOneAsync(product3);

        var product4 = new Product
        {
            Id = ObjectId.Parse("804c3b89ae02a3135d6429fc"),
            Name = "Cabbage",
            Count = 1,
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // See above
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await productsCollection.InsertOneAsync(product4);

        var product5 = new Product
        {
            Id = ObjectId.Parse("805c3b89ae02a3135d6429fc"),
            Name = "Potatoes",
            Count = 2,
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // See above
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await productsCollection.InsertOneAsync(product5);

        var product6 = new Product
        {
            Id = ObjectId.Parse("806c3b89ae02a3135d6429fc"),
            Name = "Carrots",
            Count = 2,
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // See above
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await productsCollection.InsertOneAsync(product6);

        var product7 = new Product
        {
            Id = ObjectId.Parse("807c3b89ae02a3135d6429fc"),
            Name = "Onion",
            Count = 1,
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // See above
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await productsCollection.InsertOneAsync(product7);

        var product8 = new Product
        {
            Id = ObjectId.Parse("808c3b89ae02a3135d6429fc"),
            Name = "Tomato Paste",
            Count = 2,
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc"), // See above
            CreatedById = ObjectId.Parse("652c3b89ae02a3135d6408fc"), // See above (admin@gmail.com)
            CreatedDateUtc = DateTime.UtcNow
        };
        await productsCollection.InsertOneAsync(product8);
    }

    public async Task InitializeRecipesAsync()
    {
        var recipesCollection = _dbContext.Db.GetCollection<Recipe>("Recipes");

        var recipe = new Recipe
        {
            Id = ObjectId.Parse("901c3b89ae02a3135d6429fc"),
            Name = "Cabbage Salad",
            Thumbnail = null,
            Text = "This is a simple recipe",
            Ingredients = [
                new Product { Id = ObjectId.Parse("804c3b89ae02a3135d6429fc"), Name = "Cabbage", Count = 1 },
                new Product { Id = ObjectId.Parse("807c3b89ae02a3135d6429fc"), Name = "Onion", Count = 2 },
            ],
            Categories = [
                new Category { Id = ObjectId.Parse("752c3b89ae02a3135d6429fc"), Name = "Breakfast" }
            ],
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc")
        };
        await recipesCollection.InsertOneAsync(recipe);

        var friedPotatoes = new Recipe
        {
            Id = ObjectId.Parse("902c3b89ae02a3135d6429fc"),
            Name = "Fried Potatoes",
            Thumbnail = null,
            Text = "This is a simple recipe",
            Ingredients = [
                new Product { Id = ObjectId.Parse("805c3b89ae02a3135d6429fc"), Name = "Potatoes", Count = 2 },
                new Product { Id = ObjectId.Parse("807c3b89ae02a3135d6429fc"), Name = "Onion", Count = 1 },
            ],
            Categories = [
                new Category { Id = ObjectId.Parse("752c3b89ae02a3135d6429fc"), Name = "Breakfast" }
            ],
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc")
        };
        await recipesCollection.InsertOneAsync(friedPotatoes);

        var deleteRecipe = new Recipe
        {
            Id = ObjectId.Parse("903c3b89ae02a3135d6429fc"),
            Name = "Delete",
            Thumbnail = null,
            Text = "This is a simple recipe",
            Ingredients = [
                new Product { Id = ObjectId.Parse("805c3b89ae02a3135d6429fc"), Name = "Potatoes", Count = 5 },
            ],
            Categories = [
                new Category { Id = ObjectId.Parse("752c3b89ae02a3135d6429fc"), Name = "Breakfast" }
            ],
            GroupId = ObjectId.Parse("652c3b89ae02a3135d6429fc")
        };
        await recipesCollection.InsertOneAsync(deleteRecipe);
    }
}
