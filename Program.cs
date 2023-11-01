using Play.Common.Data;
using Play.Common.Repositories;
using Play.Common.Repositories.Interfaces;
using Play.Inventory.Clients;
using Play.Inventory.Data.Contexts;
using Play.Inventory.Entities;
using Polly;
using Polly.Timeout;

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

var jitter = new Random();
 
// catalog client 
builder.Services.AddHttpClient<CatalogClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5000");
})
    .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.Or<TimeoutRejectedException>().WaitAndRetryAsync(5, 
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitter.Next(0, 1000)),
        onRetry: (outcome, timespan, retryAttempt) =>
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            serviceProvider.GetService<ILogger<CatalogClient>>()!.LogWarning(
                "Delaying for {delay}ms, then making retry {retry}.",
                timespan.TotalMilliseconds,
                retryAttempt
            );
        }
        ))
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(2)));

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