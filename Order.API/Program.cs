using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Application.Consumers;
using Order.Domain.Entities;
using Order.Domain.Interfaces;
using Order.Infrastructure.Context;
using Order.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Order.Application.Responses.OrderResponse).Assembly);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();


builder.Services.AddMassTransit(config =>
{
    // Tüketici (Consumer) sınıfımızı sisteme tanıtıyoruz
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        // RabbitMQ sunucu adresimiz
        cfg.Host("amqp://guest:guest@localhost:5672");

        // Hangi kuyruğu dinleyeceğimizi ve bu kuyruğa kimin bakacağını belirtiyoruz
        cfg.ReceiveEndpoint("basket-checkout-queue", c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
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
