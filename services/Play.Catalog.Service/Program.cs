using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set the GuidRepresentationMode to V3 (Standard)
BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V2;

builder.Services.AddScoped<IItemsRepository, ItemsRepository>();

// Add gRPC services to the container
builder.Services.AddGrpc();

var app = builder.Build();

// builder.WebHost.ConfigureKestrel(options =>
// {
//     options.ConfigureHttpsDefaults(httpsOptions =>
//     {
//         httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls13 | System.Security.Authentication.SslProtocols.Tls12;
//     });
// });

// Map the gRPC service to handle incoming requests
app.MapGrpcService<CatalogService>();
app.MapGet("/", () => "gRPC Catalog service is running");



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