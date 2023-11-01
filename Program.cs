using Play.Common.Data;
using Play.Common.Repositories;
using Play.Common.Repositories.Interfaces;
using Play.Inventory.Clients;
using Play.Inventory.Data.Contexts;
using Play.Inventory.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// context: inventory context
builder.Services.AddScoped< IItemContext<InventoryItem>, InventoryContext>();

// repository
builder.Services.AddScoped<IItemRepository<InventoryItem>, ItemRepository<InventoryItem>>();
 
// catalog client
builder.Services.AddHttpClient<CatalogClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();

app.Run();