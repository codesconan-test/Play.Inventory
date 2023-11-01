using Microsoft.AspNetCore.Mvc;
using Play.Common.Repositories.Interfaces;
using Play.Inventory.DTOs;
using Play.Inventory.Entities;

namespace Play.Inventory.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemRepository<InventoryItem> _repository;
    
    public ItemsController(IItemRepository<InventoryItem> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DtOs.InventoryItemDto>>> GetAsync(Guid userId)
    {
        var items = (await _repository.GetAllAsync(item => item.UserId == userId)).Select(item => item.AsDto());
        return Ok(items);
    }
    
    [HttpPost]
    public async Task<ActionResult> PostAsync(DtOs.GrantItemsDto grantItems)
    {
        var inventoryItem = await _repository.GetAsync(item =>
            item.UserId == grantItems.UserId && item.CatalogItemId == grantItems.CatalogItemId);

        if (inventoryItem == null)
        {
            inventoryItem = new InventoryItem
            {
                CatalogItemId = grantItems.CatalogItemId,
                UserId = grantItems.UserId,
                Quantity = grantItems.Quantity,
                AcquiredDate = DateTimeOffset.UtcNow.Date
            };
            
            await _repository.CreateAsync(inventoryItem);
        }
        else
        {
            inventoryItem.Quantity += grantItems.Quantity;
            await _repository.UpdateAsync(inventoryItem.UserId, inventoryItem);
            
        }
        
        return Ok();
        
    }
    
}