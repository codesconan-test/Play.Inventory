using Play.Inventory.DTOs;
using Play.Inventory.Entities;

namespace Play.Inventory;

public static class Extensions
{
    public static DtOs.InventoryItemDto AsDto(this InventoryItem item, string name, string description)
    {
        return new DtOs.InventoryItemDto(item.CatalogItemId, item.Quantity, item.AcquiredDate, name, description);
    }
}