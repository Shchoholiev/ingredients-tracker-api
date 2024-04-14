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
}
