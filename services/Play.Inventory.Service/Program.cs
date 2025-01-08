using Amazon.Auth.AccessControlPolicy;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Play.Catalog;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Repositories;

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


//Inter-services Communication using REST and HTTP
builder.Services.AddHttpClient<CatalogClient>(client =>{
    client.BaseAddress= new Uri("http://localhost:5205");
});
// .AddPolicyHandler(Policy.TimeoutAsync<HttpRequestMessage>(1)); //one second timeout

// Add gRPC services to the container
// builder.Services.AddGrpc();

builder.Services.AddScoped<CatalogClientI>();

// Register gRPC client (Inter-services Communication)
builder.Services.AddGrpcClient<Catalog.CatalogClient>(o =>
{
    o.Address = new Uri("https://localhost:5205"); // Address of the catalog service
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
