using Play.Inventory.DTOs;

namespace Play.Inventory.Clients;

public class CatalogClient
{
    private readonly HttpClient _httpClient;
    
    public CatalogClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    // get catalog item
    public async Task<DtOs.CatalogItemDto?> GetCatalogItemAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"items/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<DtOs.CatalogItemDto>();
        }
        return null;
    }
}