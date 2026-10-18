using Basket.API.Protos;
using Basket.API.Services;
using Basket.Domain.Interfaces;
using Basket.Infrastructure.Repositories;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Basket.Application.Features.Baskets.Query.GetBasketQuery).Assembly);
});

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        // docker-compose.yml dosyamızdaki RabbitMQ bilgilerini kullanıyoruz
        cfg.Host("amqp://guest:guest@localhost:5672");
    });
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<ICatalogGrpcService, CatalogGrpcService>();

builder.Services.AddGrpcClient<ProductProtoService.ProductProtoServiceClient>(options =>
{
    
    options.Address = new Uri("https://localhost:7244");
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
