using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MassTransit;
using Play.Catalog.Service.Settings;
using MassTransit.JobService;
using Play.Contracts;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Set the GuidRepresentationMode to V3 (Standard)
BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V2;

builder.Services.AddScoped<IItemsRepository, ItemsRepository>();

//MassTransit confif for RabbitMQ
builder.Services.AddMassTransit(configure =>
{
    configure.UsingRabbitMq((context,configurator) =>
    {
        // var configuration = context.GetService<IConfiguration>();
        // var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
        // var rabbitMQSettings = builder.Configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        // configurator.Host(rabbitMQSettings.HostName);

        configurator.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        configurator.ConfigureEndpoints(context);
        //, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

    });
});

//Start the mass transit to publish the messages
// builder.Services.AddMassTransitHostedService();


// //for Grpc to use Http2
// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ListenLocalhost(5205, listenOptions =>
//     {
//         listenOptions.UseHttps();
//         listenOptions.Protocols = HttpProtocols.Http1AndHttp2;  // Enable both HTTP/1.1 and HTTP/2

//     });
// });

// // Add gRPC services to the container
// builder.Services.AddGrpc();

var app = builder.Build();

// // Map the gRPC service to handle incoming requests
// app.MapGrpcService<CatalogService>();
// app.MapGet("/", () => "gRPC Catalog service is running");



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