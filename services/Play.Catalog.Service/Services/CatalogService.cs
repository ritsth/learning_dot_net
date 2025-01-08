using Grpc.Core;
using Play.Catalog; // This is where the CatalogBase class should reside
using Microsoft.AspNetCore.Mvc;

namespace Play.Catalog.Service.Services
{
    public class CatalogService : Catalog.CatalogBase
    {
        private readonly ILogger<CatalogService> _logger;

        public CatalogService(ILogger<CatalogService> logger){
            _logger = logger;
        }

        public override Task<ProductDetails> GetProductDetails(ProductRequest request, ServerCallContext context)
        {
            //get the catalog name and description using the catalog id
            // ...


            // Simulate fetching product details
            return Task.FromResult(new ProductDetails
            {
                ProductId = request.ProductId,
                Name = "Sample Product",
                Description = "A sample product for testing.",
            });
        }

        public override async Task GetAllProductDetails(AllProductRequest request, IServerStreamWriter<ProductDetails> responseStream, ServerCallContext context)
        {
            //Stream all the products for the client
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(new ProductDetails
                {
                    ProductId = Guid.NewGuid().ToString(),
                    Name = "Sample Product",
                    Description = "A sample product for testing.",
                });
            };
        }
    }
    
}
