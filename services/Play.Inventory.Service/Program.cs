using Amazon.Auth.AccessControlPolicy;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Play.Catalog;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Repositories;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MassTransit;
using Play.Inventory.Service.Consumer;
using MassTransit.RabbitMqTransport;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Conversion from guid while storing in the database
BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V2;

//Dependency injection (interface)
builder.Services.AddScoped<IItemsRepository, ItemsRepository>();
builder.Services.AddScoped<ICatalogItemsRepository, CatalogItemsRepository>();

//Inter-services Communication using REST and HTTP1
builder.Services.AddHttpClient<CatalogClient>(client =>{
    client.BaseAddress= new Uri("http://localhost:5205");
});
//Handeling partial failure when communicating with other microservices 
//1. Timeouts and Retry Policies (using Polly)
// .AddPolicyHandler(Policy.TimeoutAsync<HttpRequestMessage>(1)); //one second timeout
//2. Circuit Breaker Pattern
// 3. Asynchronous Communication (implementted this using RabbitMQ)


// //Grpc Communication stuff
// builder.Services.AddScoped<CatalogClientGrpc>();

// // Register gRPC client (Inter-services Communication)
// builder.Services.AddGrpcClient<Catalog.CatalogClient>(o =>
// {
//     o.Address = new Uri("https://localhost:5205"); // Address of the catalog service
// });


//Configure RabbitMQ
builder.Services.AddMassTransit(configure =>
{        
    // Register the consumer
    configure.AddConsumer<CatalogItemCreatedConsumer>();
    configure.AddConsumer<CatalogItemUpdatedConsumer>();

    configure.UsingRabbitMq((context,configurator) =>
    {
        //not default so we need to configure it
        // configurator.Host("rabbitmq", 5673, "/", h =>
        // {
        //     h.Username("guest");
        //     h.Password("guest");
        // });
        configurator.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Configure consumer endpoints
        configurator.ReceiveEndpoint("my-message-queue", e =>
        {
            e.ConfigureConsumer<CatalogItemCreatedConsumer>(context);
            e.ConfigureConsumer<CatalogItemUpdatedConsumer>(context);

        });
    });
});

// Register MassTransit-related services
builder.Services.AddMassTransitHostedService();


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
