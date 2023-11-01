using Microsoft.AspNetCore.Mvc;
using Play.Common.Repositories.Interfaces;
using Play.Inventory.Clients;
using Play.Inventory.DTOs;
using Play.Inventory.Entities;

namespace Play.Inventory.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemRepository<InventoryItem> _repository;
    // catalog client
    private readonly CatalogClient _catalogRepository;
    
    public ItemsController(IItemRepository<InventoryItem> repository, CatalogClient catalogRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _catalogRepository = catalogRepository;
    }
    
    
    // get all
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<DtOs.InventoryItemDto>>> GetAllAsync()
    {
        var inventoryItemEntities = await _repository.GetAllAsync();
        
        
        
        return Ok(inventoryItemEntities);
    }
    
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DtOs.InventoryItemDto>>> GetAsync(Guid userId)
    {
        if(userId == Guid.Empty)
        {
            return BadRequest();
        }
        
        var inventoryItemEntities = await _repository.GetAllAsync(item => item.UserId == userId);
        
        var inventoryItemDtos = inventoryItemEntities.Select(async item =>
        {
            var catalogItem = await _catalogRepository.GetCatalogItemAsync(item.CatalogItemId);
            var inventoryItemDto = item.AsDto(catalogItem?.Name!, catalogItem?.Description!);
            return inventoryItemDto;
        }).Select(task => task.Result);
        
        return Ok(inventoryItemDtos);
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