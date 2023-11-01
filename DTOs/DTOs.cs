namespace Play.Inventory.DTOs;

public class DtOs
{
    public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Quantity);
    
    public record InventoryItemDto(Guid CatalogItemId, int Quantity, DateTime AcquiredDate, string Name, string Description);
    
    // catalog item
    public record CatalogItemDto(Guid Id, string Name, string Description, int Price);
}