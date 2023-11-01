using MongoDB.Driver;
using Play.Common.Data;
using Play.Inventory.Entities;

namespace Play.Inventory.Data.Contexts;

public class InventoryContext : IItemContext<InventoryItem>
{
    public InventoryContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        
        Items = database.GetCollection<InventoryItem>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        
        // InventoryContextSeed.SeedData(Items);
    }
    
    public IMongoCollection<InventoryItem> Items { get; }
}