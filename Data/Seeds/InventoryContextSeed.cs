using MongoDB.Driver;
using Play.Inventory.Entities;

namespace Play.Inventory.Data.Seeds;

public class InventoryContextSeed
{
    // seed data
    public static void SeedData(IMongoCollection<InventoryItem> items)
    {
        var existItem = items.Find(item => true).Any();
        if (!existItem)
        {
            items.InsertManyAsync(GetPreconfiguredItems());
        }
    }

    private static IEnumerable<InventoryItem> GetPreconfiguredItems()
    {
        return new List<InventoryItem>
        {
            
        };
    }
}