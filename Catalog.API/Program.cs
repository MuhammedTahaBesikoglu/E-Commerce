using Catalog.API.GrpcServices;
using Catalog.Application.Behaviors;
using Catalog.Application.Consumers;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Context;
using Catalog.Infrastructure.Repositories;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMediatR(cfg => 
{ 
    cfg.RegisterServicesFromAssembly(typeof(Catalog.Application.Responses.ProductResponse).Assembly);
    cfg.AddOpenBehavior(typeof(Catalog.Application.Behaviors.ValidationBehavior<,>));

});

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<OrderCreatedConsumer>();
    config.AddConsumer<OrderCancelledConsumer>();
    config.AddConsumer<PaymentFailedConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");

        // Katalog servisinin dinleyeceği özel kuyruk
        cfg.ReceiveEndpoint("order-created-queue", c =>
        {
            c.ConfigureConsumer<OrderCreatedConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("order-cancelled-queue", c =>
        {
            c.ConfigureConsumer<OrderCancelledConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("catalog-payment-failed-queue", c =>
        {
            c.ConfigureConsumer<PaymentFailedConsumer>(ctx);
        });
    });
});

builder.Services.AddValidatorsFromAssembly(typeof(Catalog.Application.Responses.ProductResponse).Assembly);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<Catalog.API.Middlewares.GlobalExceptionMiddleware>();

app.MapGrpcService<ProductGrpcService>();

app.UseAuthorization();

app.MapControllers();

app.Run();
